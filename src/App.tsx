// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import AdminPage from './pages/AdminPage';
import Test from './components/AdminBook';
import CustomerHomePage from './pages/CustomerHomePage'
import CustomerProductDetailPage from './pages/CustomerProductDetailPage';
import CustomerCartPage from './pages/CustomerCartPage';
import CustomerConfirmOrderPage from './pages/CustomerConfirmOrderPage';
import CustomerOrderPage from './pages/CustomerOrderPage';
const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route path="/admin" element={<AdminPage />} />
        <Route path="/test" element={<Test />} />
        <Route path="" element={<CustomerHomePage />} />
        <Route path="/:slug/:id" element={<CustomerProductDetailPage />} />
        <Route path="/cart" element={<CustomerCartPage />} />
        <Route path="/order" element={<CustomerOrderPage />} />
        <Route path="/order-confirm" element={<CustomerConfirmOrderPage />} />

      </Routes>
    </Router>
  );
};

export default App;
