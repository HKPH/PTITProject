import React, { useState, useEffect } from 'react';
import { Table, Input, Pagination, message, Button, Modal, Tabs } from 'antd';
import { getAccounts, toggleAccountActive, resetPassword } from '../api/accountApi';
import { getUserInfo } from '../api/userApi';
import dayjs from 'dayjs';

export interface Account {
  id: number;
  username: string;
  password: string;
  role: number;
  createDate: string;
  active: boolean;
}

export interface UserInfo {
  id: number;
  name: string;
  phone: string;
  email: string;
  address: string;
  dob: string;
  gender: number;
  accountId: number;
  active: boolean;
}

const AccountList: React.FC = () => {
  const [accounts, setAccounts] = useState<Account[]>([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [totalAccounts, setTotalAccounts] = useState(0);
  const [searchUsername, setSearchUsername] = useState<string | null>(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [modalTabKey, setModalTabKey] = useState('personal');
  const [selectedAccount, setSelectedAccount] = useState<Account | null>(null);
  const [userInfo, setUserInfo] = useState<UserInfo | null>(null);

  useEffect(() => {
    fetchAccounts();
  }, [page, searchUsername]);

  const fetchAccounts = async () => {
    setLoading(true);
    try {
      const data = await getAccounts(page, pageSize, searchUsername);
      setAccounts(data.items);
      setTotalAccounts(data.totalCount);
    } catch (error) {
      message.error("Failed to load accounts");
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (value: string) => {
    setSearchUsername(value || null);
    setPage(1);
  };

  const handleToggleActive = async (accountId: number, active: boolean) => {
    try {
      await toggleAccountActive(accountId);
      message.success(`Tài khoản đã ${active ? 'tắt' : 'bật'} hoạt động`);
      fetchAccounts();
    } catch (error) {
      message.error("Không thể thay đổi trạng thái tài khoản");
    }
  };

  const handleResetPassword = async (accountId: number) => {
    try {
      await resetPassword(accountId);
      message.success('Mật khẩu đã được reset');
    } catch (error) {
      message.error("Không thể reset mật khẩu");
    }
  };

  const handleShowDetails = async (account: Account) => {
    setSelectedAccount(account);
    setIsModalVisible(true);

    // Gọi API để lấy thông tin cá nhân
    try {
      const userData = await getUserInfo(account.id);
      setUserInfo(userData);
    } catch (error) {
      message.error("Không thể tải thông tin cá nhân");
    }
  };

  const handleCancelModal = () => {
    setIsModalVisible(false);
    setUserInfo(null);
  };

  const handleTabChange = (key: string) => {
    setModalTabKey(key);
  };

  return (
    <div>
      <Input.Search
        placeholder="Tìm kiếm username"
        onSearch={handleSearch}
        style={{ width: 300, marginBottom: 20 }}
      />
      <Table
        dataSource={accounts}
        loading={loading}
        rowKey="id"
        pagination={false}
        columns={[
          { title: 'ID', dataIndex: 'id', key: 'id' },
          { title: 'Username', dataIndex: 'username', key: 'username' },
          { title: 'Role', dataIndex: 'role', key: 'role' },
          { title: 'Active', dataIndex: 'active', key: 'active', render: (active) => (active ? 'Yes' : 'No') },
          { title: 'Create Date', dataIndex: 'createDate', key: 'createDate', render: (date: string) => dayjs(date).format('DD/MM/YYYY') },
          {
            title: 'Actions',
            key: 'actions',
            render: (_, account: Account) => (
              <div>
                <Button onClick={() => handleToggleActive(account.id, account.active)} danger style={{ marginRight: 10 }}>
                  {account.active ? 'Tắt hoạt động' : 'Bật hoạt động'}
                </Button>
                <Button onClick={() => handleResetPassword(account.id)} type="primary" style={{ marginRight: 10 }}>
                  Reset mật khẩu
                </Button>
                <Button onClick={() => handleShowDetails(account)} type="link">
                  Thông tin chi tiết
                </Button>
              </div>
            )
          }
        ]}
      />
      <Pagination
        current={page}
        pageSize={pageSize}
        total={totalAccounts}
        onChange={(page) => setPage(page)}
        style={{ marginTop: 20, textAlign: 'right' }}
      />

      {/* Modal for showing details */}
      <Modal
        title={`Thông tin tài khoản: ${selectedAccount?.username}`}
        visible={isModalVisible}
        onCancel={handleCancelModal}
        footer={null}
        width={600}
      >
        <Tabs defaultActiveKey="personal" activeKey={modalTabKey} onChange={handleTabChange}>
          <Tabs.TabPane tab="Thông tin cá nhân" key="personal">
            <p><strong>Tên:</strong> {userInfo?.name}</p>
            <p><strong>Số điện thoại:</strong> {userInfo?.phone}</p>
            <p><strong>Email:</strong> {userInfo?.email}</p>
            <p><strong>Địa chỉ:</strong> {userInfo?.address}</p>
            <p><strong>Ngày sinh:</strong> {dayjs(userInfo?.dob).format('DD/MM/YYYY')}</p>
            <p><strong>Giới tính:</strong> {userInfo?.gender === 0 ? 'Nam' : 'Nữ'}</p>
            <p><strong>Trạng thái:</strong> {userInfo?.active ? 'Hoạt động' : 'Không hoạt động'}</p>
          </Tabs.TabPane>
          <Tabs.TabPane tab="Đơn hàng" key="orders">
            <p>Thông tin đơn hàng sẽ được hiển thị ở đây.</p>
          </Tabs.TabPane>
        </Tabs>
      </Modal>
    </div>
  );
};

export default AccountList;
