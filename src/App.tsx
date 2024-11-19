// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import BookAdminPage from './pages/BookAdminPage';
import CustomerPage from './pages/CustomerPage'
import ProductDetail from './pages/ProductDetail';
import ImageUploadPage from './pages/image';
const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/admin/books" element={<BookAdminPage />} />
        <Route path="/:slug/:id" element={<ProductDetail />} />
        <Route path="/home" element={<CustomerPage />} />
        <Route path="/" element={<ImageUploadPage />} />
      </Routes>
    </Router>
  );
};

export default App;
