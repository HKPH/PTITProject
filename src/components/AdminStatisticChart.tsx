import React, { useEffect, useState } from 'react';
import { Line, Pie } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, LineElement, PointElement, ArcElement, Tooltip, Legend } from 'chart.js';
import {
    getBooksSoldByDay,
    getBooksSoldByYear,
    getBooksSoldByQuarters,
    getRevenueByDay,
    getRevenueByYear,
    getRevenueByQuarters,
    getRatingsByYear,
    getRatingsByQuarters
} from '../api/statisticsApi';

ChartJS.register(CategoryScale, LinearScale, LineElement, PointElement, ArcElement, Tooltip, Legend);

interface AdminStatisticChartProps {
    selectedStat: 'books' | 'revenue' | 'ratings';
}

const AdminStatisticChart: React.FC<AdminStatisticChartProps> = ({ selectedStat }) => {
    const currentYear = new Date().getFullYear();
    const [selectedYear, setSelectedYear] = useState<number>(currentYear);
    const [selectedPeriod, setSelectedPeriod] = useState<'month' | 'quarter'>('month');
    const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);

    const [dailyData, setDailyData] = useState<{ booksSold: number; revenue: number } | null>(null);
    const [periodicData, setPeriodicData] = useState<any>(null);

    const fetchDailyStats = async () => {
        try {
            const booksResponse = await getBooksSoldByDay(selectedDate);
            const revenueResponse = await getRevenueByDay(selectedDate);
            setDailyData({ booksSold: booksResponse, revenue: revenueResponse });
        } catch (error) {
            console.error('Error fetching daily stats:', error);
        }
    };

    const fetchPeriodicData = async () => {
        let response;
        if (selectedStat === 'books') {
            if (selectedPeriod === 'month') {
                response = await getBooksSoldByYear(selectedYear);
                setPeriodicData({
                    labels: response.map((item: any) => `Tháng ${item.month}`),
                    datasets: [
                        {
                            label: 'Doanh số',
                            data: response.map((item: any) => item.booksSold),
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1,
                        },
                    ],
                });
            } else if (selectedPeriod === 'quarter') {
                response = await getBooksSoldByQuarters(selectedYear);
                setPeriodicData({
                    labels: response.map((item: any) => `Q${item.quarter}`),
                    datasets: [
                        {
                            label: 'Doanh số',
                            data: response.map((item: any) => item.booksSold),
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1,
                        },
                    ],
                });
            }
        } else if (selectedStat === 'revenue') {
            if (selectedPeriod === 'month') {
                response = await getRevenueByYear(selectedYear);
                setPeriodicData({
                    labels: response.map((item: any) => `Tháng ${item.month}`),
                    datasets: [
                        {
                            label: 'Doanh thu',
                            data: response.map((item: any) => item.revenue),
                            borderColor: 'rgb(54, 162, 235)',
                            tension: 0.1,
                        },
                    ],
                });
            } else if (selectedPeriod === 'quarter') {
                response = await getRevenueByQuarters(selectedYear);
                setPeriodicData({
                    labels: response.map((item: any) => `Q${item.quarter}`),
                    datasets: [
                        {
                            label: 'Doanh thu',
                            data: response.map((item: any) => item.revenue),
                            borderColor: 'rgb(54, 162, 235)',
                            tension: 0.1,
                        },
                    ],
                });
            }
        } else if (selectedStat === 'ratings') {
            let ratingsCount: Record<number, number> = { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 }; // Include 0 rating
            if (selectedPeriod === 'month') {
                response = await getRatingsByYear(selectedYear);
                console.log(response)
            } else if (selectedPeriod === 'quarter') {
                response = await getRatingsByQuarters(selectedYear);
                console.log(response)
            }
            response.forEach((item: any) => {
                item.ratingsCountByStar.forEach((rating: any) => {
                    ratingsCount[rating.star] = (ratingsCount[rating.star] || 0) + rating.count;
                });
            });
            setPeriodicData({
                labels: ['0 sao', '1 sao', '2 sao', '3 sao', '4 sao', '5 sao'], // Add 0 sao here
                datasets: [
                    {
                        data: [
                            ratingsCount[0], // For rating 0
                            ratingsCount[1], // For rating 1
                            ratingsCount[2], // For rating 2
                            ratingsCount[3], // For rating 3
                            ratingsCount[4], // For rating 4
                            ratingsCount[5], // For rating 5
                        ],
                        backgroundColor: ['#FF0000', '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'], // Color for each rating
                    },
                ],
            });
        }
    };

    useEffect(() => {
        fetchDailyStats();
    }, [selectedDate]);

    useEffect(() => {
        fetchPeriodicData();
    }, [selectedYear, selectedPeriod, selectedStat]);

    return (
        <div style={{ fontFamily: 'Arial, sans-serif', padding: '20px' }}>
            <h2 style={{ textAlign: 'center', color: '#333' }}>Biều đồ thống kê</h2>

            <div>
                <div style={{ marginBottom: '16px', display: 'flex', alignItems: 'center' }}>
                    <h3 style={{ marginRight: '16px' }}>Doanh số trong ngày</h3>
                    <label>
                        Ngày:
                        <input
                            type="date"
                            value={selectedDate}
                            onChange={(e) => setSelectedDate(e.target.value)}
                            style={{ marginLeft: '10px' }}
                        />
                    </label>
                </div>

                {dailyData && (
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <p style={{ margin: 0 }}>
                            <strong>Sách bán trong {selectedDate}: </strong>
                            {dailyData.booksSold}
                        </p>
                        <p style={{ margin: 0 }}>
                            <strong>Doanh số trong {selectedDate}: </strong>
                            {dailyData.revenue}
                        </p>
                    </div>
                )}

            </div>


            <div style={{ marginTop: '20px' }}>
                <h3>Doanh số</h3>
                <div>
                    <label>
                        Chọn khoảng:
                        <select
                            value={selectedPeriod}
                            onChange={(e) => setSelectedPeriod(e.target.value as 'month' | 'quarter')}
                            style={{ marginLeft: '10px' }}
                        >
                            <option value="month">Tháng</option>
                            <option value="quarter">Quý</option>
                        </select>
                    </label>
                </div>
                <div style={{ marginTop: '10px' }}>
                    <label>
                        Chọn năm:
                        <select
                            value={selectedYear}
                            onChange={(e) => setSelectedYear(Number(e.target.value))}
                            style={{ marginLeft: '10px' }}
                        >
                            {Array.from({ length: 10 }).map((_, index) => (
                                <option key={index} value={currentYear - index}>
                                    {currentYear - index}
                                </option>
                            ))}
                        </select>
                    </label>
                </div>
                {periodicData && periodicData.labels && periodicData.datasets ? (
                    <Line data={periodicData} />
                ) : (
                    <p>Loading data...</p>
                )}


            </div>
        </div>
    );
};

export default AdminStatisticChart;
