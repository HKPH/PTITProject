# **Hybrid Recommendation System for E-Commerce Bookstore**

## **Overview**

An e-commerce bookstore application featuring **Hybrid Recommendation System** to personalize book suggestions and improve user engagement. The project is modularized into distinct components: **Backend API**, **Recommendation Engine**, and **Frontend UI**, each built with scalability and maintainability.

---

## **Backend (.NET Web API)**

> **Branch:** main, BE_api (old version)
> **Tech Stack:** .NET 8, C#, Entity Framework Core, SQL Server, Redis, Docker, Azure

The backend is the core of the system, built with performance and modularity in mind. In the final phase of development, the backend was refactored and enhanced to meet production-grade standards:

* ✅ Developed **RESTful APIs** for books, users, carts, orders, ratings, authentication, and admin functions.
* ✅ Applied **Clean Architecture** with layered separation (API, Application, Infrastructure).
* ✅ Implemented **caching using Redis** for performance-critical endpoints (e.g., recommendations).
* ✅ Integrated **JWT Authentication with Refresh Tokens**.
* ✅ Deployed using **Docker** and hosted on **Microsoft Azure (App Services, Azure SQL, Azure Redis)** for scalability.
* ✅ Added **unit testing** with xUnit and mocking for key services like `AccountService`, `BookService`, and `OrderService`.
* ✅ Public Swagger UI available for testing and documenting APIs:
🔗 (https://book-web-b0fpbxf2fbd5fzcx.eastasia-01.azurewebsites.net/swagger/index.html)

## **Hybrid Recommendation System (AI Module)**

> **Branch:** `RC_System`
> **Tech Stack:** Python, Flask, Scikit-Learn, Pandas, NumPy

The AI engine exposes APIs that generate personalized recommendations using multiple strategies:

* **Content-Based Filtering (CB):**

  * Uses **TF-IDF + Cosine Similarity** to recommend books with similar metadata (title, category, author).
* **Collaborative Filtering (CF):**

  * Implements both **User-Based and Item-Based** models using user interactions (ratings).
* **Hybrid Strategies:**
  
After experimenting with multiple approaches (Weighted, Switching), the Combining Model was selected for production due to its ability to:
* Leverage both CF and CB outputs in parallel.
* Merge, rank, and filter recommendations from both sources.
* Improve diversity and relevance, especially for users with sparse data.

Exposed via **Flask REST API**, consumed directly by the backend for real-time recommendation delivery.

## **Frontend (ReactJS Admin & Client UI)**

> **Branch:** `FE_System`
> **Tech Stack:** ReactJS, Axios, Tailwind CSS

The frontend offers a clean, responsive user interface and a full-featured admin panel:

* **Client Interface:**

  * Browse/search books, view recommendations, rate and review books, manage cart and orders.
* **Admin Dashboard:**

  * Manage books, categories, publishers, user roles, and monitor system analytics.
* Integrated with backend APIs for real-time interaction.
