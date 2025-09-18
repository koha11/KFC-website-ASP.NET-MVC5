## Info
A KFC-style fried-chicken e-commerce website. The app supports multiple roles (Customer, Store Manager, Shipper, Admin), online ordering with coupons, and order tracking via AJAX updates.
## Features
- Customer
  - Browse menu (chicken, combos, sides, drinks) with filtering and sorting.
  - Cart, coupon/promo codes, address book, and checkout.
  - Order tracking with status flow: Pending → Confirmed → Preparing → OutForDelivery → Delivered/Failed.
  - AJAX add-to-cart, price recalculation, and live status refresh (no full reloads).
  - Order history and re-order.
- Store Manager
  - Menu management: products, variants, prices, availability windows.
  - Approve order and assign shipper
  - Promotions management and usage limits.
  - Order management (confirm/prepare/cancel) with activity logs.
- Shipper
  - Delivery queue with assignment, pickup, proof-of-delivery (note/photo placeholder), and status updates.
- Admin
  - User and role management (RBAC), store settings, and system metrics.
- Tech Stack
  - Backend: ASP.NET MVC 5, C#
  - Database: Microsoft SQL Server (LocalDB or SQL Server Express/Standard)
  - ORM: Entity Framework 6 (Code-First or Database-First)
  - Frontend: Bootstrap 5, jQuery, AJAX
## Demo
