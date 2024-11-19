import React, { useState } from 'react';
import axios from 'axios';

const ImageUpload = () => {
  const [file, setFile] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState<string>('');

  // Handle file input change
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFile(e.target.files[0]);
    }
  };

  // Handle file upload
  const handleUpload = async () => {
    if (!file) {
      alert('Vui lòng chọn một file ảnh để tải lên');
      return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
      // Gửi yêu cầu POST lên API
      const response = await axios.post('https://localhost:7262/api/ImageUpload/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      // Hiển thị URL của ảnh đã tải lên
      setImageUrl(response.data.url);
    } catch (error) {
      console.error('Có lỗi khi tải ảnh lên:', error);
      alert('Lỗi khi tải ảnh lên');
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>Tải Ảnh Lên API</h1>
      <input type="file" onChange={handleFileChange} />
      <button onClick={handleUpload}>Tải Lên</button>

      {imageUrl && (
        <div style={{ marginTop: '20px' }}>
          <h3>Ảnh đã tải lên:</h3>
          <img src={imageUrl} alt="Uploaded" style={{ maxWidth: '300px', maxHeight: '300px' }} />
        </div>
      )}
    </div>
  );
};

export default ImageUpload;
