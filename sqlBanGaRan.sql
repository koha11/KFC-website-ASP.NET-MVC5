CREATE DATABASE QuanLyBanGaRan_64131011

USE QuanLyBanGaRan_64131011


-- Bảng Role
CREATE TABLE AppRole (
    RoleID VARCHAR(8) PRIMARY KEY,
    RoleName NVARCHAR(256) NOT NULL,
);

-- Bảng Users
CREATE TABLE AppUser (
    UserID VARCHAR(8) PRIMARY KEY,
    FullName NVARCHAR(256) NULL,
    Email NVARCHAR(256) UNIQUE NOT NULL,
    Address NVARCHAR(256) NULL,
    Phone CHAR(10) NULL,
	CCCD CHAR(12) NULL,
	DOB DATE NULL,
	IsActived BIT DEFAULT 0,
	Avatar NVARCHAR(256) NULL,
    Username VARCHAR(30) UNIQUE NOT NULL,
    Password NVARCHAR(256) NOT NULL,	
	RoleID VARCHAR(8) NOT NULL
	FOREIGN KEY (RoleID) REFERENCES AppRole(RoleID) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Bảng FoodCategory
CREATE TABLE FoodCategory (
    FoodCategoryID CHAR(8) PRIMARY KEY,
    FoodCategoryName NVARCHAR(256) NOT NULL,
);

-- Bảng Food
CREATE TABLE Food (
    FoodID CHAR(8) PRIMARY KEY,
    FoodName NVARCHAR(256) NOT NULL,
    FoodDetails NVARCHAR(256),
    FoodPrice MONEY NOT NULL,
    FoodUnits NVARCHAR(30) NOT NULL,
    FoodImage NVARCHAR(256),
    FoodCategoryID CHAR(8) NOT NULL,
    FOREIGN KEY (FoodCategoryID) REFERENCES FoodCategory(FoodCategoryID) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Bảng Promotion
CREATE TABLE Promotion (
    PromotionID VARCHAR(32) PRIMARY KEY,
    PromotionName NVARCHAR(256) NOT NULL,
    PromotionDetails NVARCHAR(256),	
    Discount DECIMAL(3, 2) CHECK (Discount > 0) NOT NULL,  -- Hệ số phần trăm giảm giá
);

-- Bảng FoodPromotion (quan hệ giữa Promotion và Food)
CREATE TABLE FoodPromotion (
    PromotionID VARCHAR(32),
    FoodID CHAR(8),
	DateStart DATE NOT NULL,
    DateEnd DATE NOT NULL,
    PRIMARY KEY (PromotionID, FoodID, DateStart),
    FOREIGN KEY (PromotionID) REFERENCES Promotion(PromotionID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (FoodID) REFERENCES Food(FoodID) ON DELETE CASCADE ON UPDATE CASCADE
);
---- Bảng Voucher
--CREATE TABLE Voucher (
--    VoucherID VARCHAR(32) PRIMARY KEY,
--    Condition Money NULL,
--    Discount MONEY NOT NULL,
--    OfferDetails NVARCHAR(256) NULL,
--	IsActive BIT DEFAULT 0
--);

-- Bảng Orders
CREATE TABLE CustomerOrder (
    OrderID CHAR(8) PRIMARY KEY,
    OrderDate DATETIME NULL,
    ShipmentDate DATETIME NULL,
    FinishDate DATETIME NULL,
	AcceptDate DATETIME NULL,
    Status TINYINT DEFAULT 0,
    OrderedBy VARCHAR(8) NOT NULL,
	AcceptedBy VARCHAR(8) NULL,
    ShippedBy VARCHAR(8) NULL,
	--VoucherID VARCHAR(32) NULL,
	TotalPrice Money NULL,
    FOREIGN KEY (OrderedBy) REFERENCES AppUser(UserID),
	FOREIGN KEY (AcceptedBy) REFERENCES AppUser(UserID),
    FOREIGN KEY (ShippedBy) REFERENCES AppUser(UserID)
	--FOREIGN KEY (VoucherID) REFERENCES Voucher(VoucherID)
);

-- Bảng OrderDetails (quan hệ giữa Orders và Food)
CREATE TABLE OrderDetail (
	OrderID CHAR(8),
	FoodID CHAR(8),	
	Amount TINYINT NOT NULL,
	PRIMARY KEY (OrderID, FoodID),
	FOREIGN KEY (OrderID) REFERENCES CustomerOrder(OrderID),
	FOREIGN KEY (FoodID) REFERENCES Food(FoodID)
);

-----------------------------------------



-- Tạo dữ liệu mẫu cho bảng OrderDetail

-- Nhập bảng Voucher
--INSERT INTO Voucher (VoucherID, Discount, Condition)
--VALUES('GIAMGIA10K',10000,70000),('GIAMGIA50K',50000,300000),('GIAMGIA100K',100000,600000),('GIAMGIA200K',200000,1000000);

-- Nhập bảng Promotion
INSERT INTO Promotion (PromotionID, PromotionName, PromotionDetails, Discount)
VALUES('GIAMGIA10%',N'Giảm giá 10%',NULL,0.1),
('GIAMGIA5%',N'Giảm giá 5%',NULL,0.05),
('GIAMGIA15%',N'Giảm giá 15%',NULL,0.15),
('GIAMGIA20%',N'Giảm giá 20%',NULL,0.2),
('GIAMGIA50%',N'Giảm giá cực sốc 50%',NULL,0.5),
('GIAMGIA3%',N'Giảm giá 3%',NULL,0.03)

-- Thêm roles
INSERT INTO AppRole (RoleID, RoleName)
VALUES 
('CUSTOMER', N'Khách hàng'),
('MANAGER', N'Quản lý trang web'),
('SHIPPER', N'Giao hàng'),
('ADMIN', N'Admin');

INSERT INTO AppUser (UserID, FullName, Email, Avatar, Username, Password, RoleID, IsActived, DOB, CCCD, Phone)
VALUES
(N'NV000000', N'ADMIN', 'khoa88108@gmail.com', 'employee.jpg', 'admin', '123456', 'ADMIN', 1, null, null, null),
(N'NV000001', N'Nguyễn Văn Hải', 'hai.nguyen@gmail.com', 'employee.jpg', 'hai.nguyen', 'password1', 'MANAGER', 1, '1995-03-15', '012345678901', '0912345678'),
(N'NV000002', N'Lê Thị Hạnh', 'hanh.le@gmail.com', 'employee.jpg', 'hanh.le', 'password2', 'MANAGER', 1, '1996-08-22', '012345678902', '0987654321'),
(N'NV000003', N'Trần Văn Minh', 'minh.tran@gmail.com', 'employee.jpg', 'minh.tran', 'password3', 'MANAGER', 1, '1994-07-10', '012345678903', '0901234567'),
(N'NV000004', N'Phạm Thị Hoa', 'hoa.pham@gmail.com', 'employee.jpg', 'hoa.pham', 'password4', 'MANAGER', 1, '1998-02-28', '012345678904', '0934567890'),
(N'NV000005', N'Hoàng Văn Tú', 'tu.hoang@gmail.com', 'employee.jpg', 'tu.hoang', 'password5', 'SHIPPER', 1, '2000-06-15', '012345678905', '0923456789'),
(N'NV000006', N'Đặng Thị Linh', 'linh.dang@gmail.com', 'employee.jpg', 'linh.dang', 'password6', 'SHIPPER', 1, '1999-12-05', '012345678906', '0945671234'),
(N'NV000007', N'Bùi Văn Lâm', 'lam.bui@gmail.com', 'employee.jpg', 'lam.bui', 'password7', 'SHIPPER', 1, '2001-04-20', '012345678907', '0888123456'),
(N'NV000008', N'Vũ Thị Thu', 'thu.vu@gmail.com', 'employee.jpg', 'thu.vu', 'password8', 'SHIPPER', 1, '2002-09-15', '012345678908', '0962345678'),
(N'NV000009', N'Ngô Văn Tài', 'tai.ngo@gmail.com', 'employee.jpg', 'tai.ngo', 'password9', 'SHIPPER', 1, '2003-01-10', '012345678909', '0973456789'),
(N'NV000010', N'Tô Thị Nhung', 'nhung.to@gmail.com', 'employee.jpg', 'nhung.to', 'password10', 'SHIPPER', 1, '1995-11-25', '012345678910', '0909876543');


INSERT INTO AppUser (UserID, FullName, Email, Address, Phone, Username, Password, RoleID, IsActived)
VALUES 
(N'KH000001', N'Nguyễn Văn An', 'an.nguyen@gmail.com', N'100 đường Phương Sài, phường Ngọc Hiệp', '0912345678', 'an.nguyen', 'password1', 'CUSTOMER', 1),
(N'KH000002', N'Lê Thị Hoa', 'hoa.le@gmail.com', N'200 đường Nguyễn Trãi, phường Phước Tân', '0987654321', 'hoa.le', 'password2', 'CUSTOMER', 1),
(N'KH000003', N'Trần Đình Nam', 'nam.tran@gmail.com', N'50 đường Pasteur, phường Xương Huân', '0901234567', 'nam.tran', 'password3', 'CUSTOMER', 1),
(N'KH000004', N'Phạm Thị Mai', 'mai.pham@gmail.com', N'300 đường Thống Nhất, phường Vạn Thắng', '0934567890', 'mai.pham', 'password4', 'CUSTOMER', 1),
(N'KH000005', N'Hoàng Văn Bình', 'binh.hoang@gmail.com', N'10 đường Lý Tự Trọng, phường Vạn Thắng', '0923456789', 'binh.hoang', 'password5', 'CUSTOMER', 1),
(N'KH000006', N'Đặng Thị Hà', 'ha.dang@gmail.com', N'120 đường Hùng Vương, phường Lộc Thọ', '0945671234', 'ha.dang', 'password6', 'CUSTOMER', 1),
(N'KH000007', N'Bùi Văn Long', 'long.bui@gmail.com', N'22 đường Trần Phú, phường Lộc Thọ', '0888123456', 'long.bui', 'password7', 'CUSTOMER', 1),
(N'KH000008', N'Vũ Thị Lan', 'lan.vu@gmail.com', N'75 đường Nguyễn Thiện Thuật, phường Tân Lập', '0962345678', 'lan.vu', 'password8', 'CUSTOMER', 1),
(N'KH000009', N'Lý Văn Hùng', 'hung.ly@gmail.com', N'65 đường Tô Hiến Thành, phường Tân Lập', '0973456789', 'hung.ly', 'password9', 'CUSTOMER', 1),
(N'KH000010', N'Doãn Thị Phương', 'phuong.doan@gmail.com', N'180 đường Hoàng Văn Thụ, phường Phước Long', '0909876543', 'phuong.doan', 'password10', 'CUSTOMER', 1),
(N'KH000011', N'Ngô Văn Minh', 'minh.ngo@gmail.com', N'15 đường 2/4, phường Vĩnh Phước', '0918765432', 'minh.ngo', 'password11', 'CUSTOMER', 1),
(N'KH000012', N'Phan Thị Tuyết', 'tuyet.phan@gmail.com', N'35 đường 23/10, phường Ngọc Hiệp', '0932341234', 'tuyet.phan', 'password12', 'CUSTOMER', 1),
(N'KH000013', N'Tô Văn Kiên', 'kien.to@gmail.com', N'85 đường Hồng Bàng, phường Tân Lập', '0891234567', 'kien.to', 'password13', 'CUSTOMER', 1),
(N'KH000014', N'Đinh Thị Xuân', 'xuan.dinh@gmail.com', N'12 đường Trần Nhật Duật, phường Phương Sơn', '0928765432', 'xuan.dinh', 'password14', 'CUSTOMER', 1),
(N'KH000015', N'Lâm Văn Dũng', 'dung.lam@gmail.com', N'60 đường Bạch Đằng, phường Phương Sài', '0987123456', 'dung.lam', 'password15', 'CUSTOMER', 1),
(N'KH000016', N'Võ Thị Bích', 'bich.vo@gmail.com', N'140 đường Lê Hồng Phong, phường Phước Hòa', '0945123456', 'bich.vo', 'password16', 'CUSTOMER', 1),
(N'KH000017', N'Hồ Văn Hiệp', 'hiep.ho@gmail.com', N'250 đường Nguyễn Đức Cảnh, phường Phước Long', '0912341111', 'hiep.ho', 'password17', 'CUSTOMER', 1),
(N'KH000018', N'Trương Thị Nga', 'nga.truong@gmail.com', N'310 đường Nguyễn Chí Thanh, phường Vĩnh Hải', '0937776665', 'nga.truong', 'password18', 'CUSTOMER', 1),
(N'KH000019', N'Thái Văn Khang', 'khang.thai@gmail.com', N'27 đường Võ Trứ, phường Tân Lập', '0909123456', 'khang.thai', 'password19', 'CUSTOMER', 1),
(N'KH000020', N'Phùng Thị Hạnh', 'hanh.phung@gmail.com', N'125 đường Lê Thánh Tôn, phường Lộc Thọ', '0976543210', 'hanh.phung', 'password20', 'CUSTOMER', 1);

INSERT INTO FoodCategory (FoodCategoryID, FoodCategoryName)
VALUES
    ('FC000001', N'Ưu đãi'),
    ('FC000002', N'Món mới'),
    ('FC000003', N'Combo 1 người'),
    ('FC000004', N'Combo nhóm'),
    ('FC000005', N'Gà rán - Gà quay'),
	('FC000006', N'Burger - Cơm - Mì ý'),
	('FC000007', N'Thức ăn nhẹ'),
	('FC000008', N'Thức uống tráng miệng');

INSERT INTO Food (FoodID, FoodName, FoodDetails, FoodPrice, FoodUnits, FoodImage, FoodCategoryID)
VALUES
    ('F0000001', N'Combo Lễ Hội A', N'2 Miếng Gà + 1 Ly Pepsi (lớn)', 87000, 'Combo', 'Festive-A.jpg', 'FC000001'),
    ('F0000002', N'Combo Phát Lộc', N'2 Miếng Gà + 3 Gà miếng Nuggets + 1 Ly Pepsi (vừa)', 109000, 'Combo', 'Combo-PhatLoc.jpg', 'FC000001'),
    ('F0000003', N'Combo Phát tài', N'4 Miếng Gà + 1 Mì Ý Gà viên + 2 Ly Pepsi (vừa)', 204000, 'Combo', 'Combo-PhatTai.jpg', 'FC000001'),
	('F0000004', N'Cơm Gà Viên Nanban', N'01 Cơm Gà Viên Nanban', 39000, N'Phần', 'NANBAN.jpg', 'FC000002'),
	('F0000005', N'Combo Cơm Gà Viên Nanban HDA', N'01 Cơm Gà Viên Nanban + 01 Miếng Gà Rán + 01 Pepsi lon', 86000, N'Combo', 'COMBO-A-Nan.jpg', 'FC000002'),
	('F0000006', N'Combo Cơm Gà Viên Nanban HDB', N'01 Cơm Gà Viên Nanban + 03 Miếng Gà Rán + 01 Khoai tây chiên (vừa)/ 01 Khoai tây nghiền & 01 Bắp cải trộn (vừa) + 02 Pepsi lon', 189000, N'Combo', 'COMBO-B-Nan.jpg', 'FC000002'),
	('F0000007', N'Combo Gà Rán 1', N'1 Miếng Gà + 1 Khoai Tây Chiên / 1 Khoai Tây Nghiền & Bắp Cải Trộn + 1 Pepsi (lớn)', 59000, 'Combo', 'D-CBO-CHICKEN-1.jpg', 'FC000003'),
	('F0000008', N'Combo Gà Rán 2', N'2 Miếng Gà + 1 Khoai Tây Chiên / 1 Khoai Tây Nghiền & Bắp Cải Trộn + 1 Pepsi (lớn)', 89000, 'Combo', 'D-CBO-CHICKEN-2.jpg', 'FC000003'),
	('F0000009', N'Combo Gà Quay', N'1 Đùi Gà Quay Flava + 1 Salad Hạt + 1 Lipton (lớn)', 117000, 'Combo', 'D-CBO-Big-Juicy.jpg', 'FC000003'),
	('F0000010', N'Combo Burger Tôm', N'1 Burger Tôm + 1 Khoai Tây Chiên (vừa) + 1 Pepsi (lớn)', 67000, 'Combo', 'D-CBO-B-Shrimp.jpg', 'FC000003'),
	('F0000011', N'Combo Mì Ý Gà Viên', N'1 Mì Ý Popcorn + 1 Pepsi (lớn)', 47000, 'Combo', 'D-CBO-PastaPop2.jpg', 'FC000003'),
	('F0000012', N'Box Meal Pasta Popcorn', N'1 Mì Ý Popcorn + 1 Miếng Gà + 1 Pepsi (lớn)', 78000, 'Combo', 'D-CBO-PastaPop.jpg', 'FC000003'),
	('F0000013', N'Box Meal Pasta COB', N'1 Mì Ý Gà Rán + 3 Gà Miếng Nuggets + 1 Pepsi (lớn)', 97000, 'Combo', 'Pasta-Nuggets.jpg', 'FC000003'),
	('F0000014', N'Combo Cơm Gà Teriyaki', N'1 Cơm Teriyaki + 1 Súp Rong Biển + 1 Pepsi (lớn)', 71000, 'Combo', 'D-CBO-Rice-Teri.jpg', 'FC000003'),
	('F0000015', N'Combo nhóm 2', N'2 Miếng Gà + 1 Burger Zinger + 2 Pepsi (lớn)', 127000, 'Combo', 'D-CBO-Bucket-1.jpg', 'FC000004'),
	('F0000016', N'Combo nhóm 3', N'3 Miếng Gà + 1 Mì Ý Popcorn + 1 Khoai Tây Chiên (vừa) + 2 Pepsi (lớn)', 160000, 'Combo', 'D-CBO-Bucket-2.jpg', 'FC000004'),
	('F0000017', N'Combo nhóm 4', N'4 Miếng Gà + 1 Khoai Tây Múi Cau (vừa) + 2 Pepsi (lớn)', 167000, 'Combo', 'D-CBO-Bucket-3.jpg', 'FC000004'),
	('F0000018', N'Combo nhóm 5', N'5 Miếng Gà + 1 Khoai Tây Chiên (vừa) + 3 Pepsi (lớn)', 205000, 'Combo', 'D-CBO-Bucket-4.jpg', 'FC000004'),
	('F0000019', N'Combo nhóm 6', N'6 Miếng Gà + 1 Mì Ý Popcorn + 1 Khoai Tây Múi Cau (vừa) + 4 Pepsi (lớn)', 275000, 'Combo', 'D-CBO-Bucket-5.jpg', 'FC000004'),
	('F0000020', N'1 Miếng Gà Rán', N'1 Miếng Gà Giòn Cay/Gà Truyền Thống/Gà Giòn Không Cay', 35000, N'Phần', '1-Fried-Chicken.jpg', 'FC000005'),
	('F0000021', N'6 Miếng Gà Rán', N'6 Miếng Gà Giòn Cay/Gà Truyền Thống/Gà Giòn Không Cay', 204000, N'Phần', '6-Fried-Chicken-new.jpg', 'FC000005'),
	('F0000022', N'1 Miếng Phi-lê Gà Quay', N'1 Miếng Phi-lê Gà Quay Flava/Phi-lê Gà Quay Tiêu', 39000, N'Phần', 'MOD-PHI-LE-GA-QUAY.jpg', 'FC000005'),
	('F0000023', N'Gà Viên (Lớn)', N'Gà Viên (Lớn)', 64000, N'Phần', 'POP-L.jpg', 'FC000005'),
	('F0000024', N'10 Gà Rán Tenders Vị Nguyên Bản', N'10 Gà Rán Tenders Vị Nguyên Bản', 129000, N'Phần', '10-TENDERS.jpg', 'FC000005'),
	('F0000025', N'Burger Zinger', N'1 Burger Zinger', 54000, N'Phần', 'Burger-Flava.jpg', 'FC000006'),
	('F0000026', N'Burger Gà Quay Flava', N'1 Burger Gà Quay Flava', 54000, N'Phần', 'Burger-Zinger.jpg', 'FC000006'),
	('F0000027', N'Cơm Gà Rán', N'1 Cơm Gà Rán', 48000, N'Phần', 'Rice-F.Chicken.jpg', 'FC000006'),
	('F0000028', N'Cơm Phi-lê Gà Quay', N'1 Cơm Phi-lê Gà Quay', 47000, N'Phần', 'Rice-Flava.jpg', 'FC000006'),
	('F0000029', N'Mì Ý Gà Rán', N'1 Mì Ý Gà Rán', 64000, N'Phần', 'MI-Y-GA-RAN.jpg', 'FC000006'),
	('F0000030', N'Salad Hạt', N'1 Salad Hạt', 38000, N'Phần', 'SALAD-HAT.jpg', 'FC000007'),
	('F0000031', N'3 Cá Thanh', N'3 Cá Thanh', 40000, N'Phần', '3-Fishsticks.jpg', 'FC000007'),
	('F0000032', N'Khoai Tây Chiên (Đại)', N'1 Khoai Tây Chiên (Đại)', 38000, N'Phần', 'FF-J.jpg', 'FC000007'),
	('F0000033', N'Khoai Tây Múi Cau (Vừa)', N'1 Khoai Tây Múi Cau (vừa)', 23000, N'Phần', 'khoai-mui-cau-R.jpg', 'FC000007'),
	('F0000034', N'Bắp Cải Trộn (Đại)', N'1 Bắp Cải Trộn (Đại)', 31000, N'Phần', 'CL-(J)-new.jpg', 'FC000007'),
	('F0000035', N'1 Bánh Trứng', N'1 Bánh Trứng', 18000, N'Cái', '1-eggtart.jpg', 'FC000008'),
	('F0000036', N'3 Viên Khoai Môn Kim Sa', N'3 Viên Khoai Môn Kim Sa', 34000, N'Cái', '3-taro.jpg', 'FC000008'),
	('F0000037', N'5 Thanh Bí Phô Mai', N'5 Thanh Bí Phô Mai', 65000, N'Cái', '5-Pumcheese.jpg', 'FC000008'),
	('F0000038', N'Pepsi Lon', N'1 Pepsi Lon', 19000, N'Lon', 'PEPSI_CAN.jpg', 'FC000008'),
	('F0000039', N'1 Sô-cô-la Sữa Đá', N'1 Sô-cô-la Sữa Đá', 20000, N'Cốc', 'ChoCo_Ice.jpg', 'FC000008'),
	('F0000040', N'Lipton.jpg', N'Lipton.jpg', 17000, N'Cốc', 'Lipton.jpg', 'FC000008');

--Nhập dữ liệu cho bảng FoodPromotion
INSERT INTO FoodPromotion (PromotionID, FoodID, DateStart, DateEnd)
VALUES
('GIAMGIA10%', 'F0000001', '2023-01-01', '2023-01-20'),
('GIAMGIA10%', 'F0000002', '2023-01-01', '2023-01-20'),
('GIAMGIA10%', 'F0000003', '2023-01-01', '2023-01-20'),
('GIAMGIA5%', 'F0000004', '2023-02-05', '2023-02-18'),
('GIAMGIA5%', 'F0000005', '2023-02-05', '2023-02-18'),
('GIAMGIA5%', 'F0000006', '2023-02-05', '2023-02-18'),
('GIAMGIA15%', 'F0000007', '2023-03-10', '2023-03-22'),
('GIAMGIA15%', 'F0000008', '2023-03-10', '2023-03-22'),
('GIAMGIA15%', 'F0000009', '2023-03-10', '2023-03-22'),
('GIAMGIA20%', 'F0000010', '2023-04-15', '2023-04-30'),
('GIAMGIA20%', 'F0000011', '2023-04-15', '2023-04-30'),
('GIAMGIA20%', 'F0000012', '2023-04-15', '2023-04-30'),
('GIAMGIA50%', 'F0000013', '2023-05-10', '2023-05-20'),
('GIAMGIA50%', 'F0000014', '2023-05-10', '2023-05-20'),
('GIAMGIA50%', 'F0000015', '2023-05-10', '2023-05-20'),
('GIAMGIA3%', 'F0000001', '2023-06-05', '2023-06-15'),
('GIAMGIA3%', 'F0000005', '2023-06-05', '2023-06-15'),
('GIAMGIA3%', 'F0000006', '2023-06-05', '2023-06-15'),
('GIAMGIA10%', 'F0000002', '2023-07-01', '2023-07-18'),
('GIAMGIA10%', 'F0000003', '2023-07-01', '2023-07-18'),
('GIAMGIA10%', 'F0000007', '2023-07-01', '2023-07-18'),
('GIAMGIA5%', 'F0000008', '2023-08-10', '2023-08-30'),
('GIAMGIA5%', 'F0000009', '2023-08-10', '2023-08-30'),
('GIAMGIA5%', 'F0000010', '2023-08-10', '2023-08-30'),
('GIAMGIA15%', 'F0000011', '2023-09-01', '2023-09-20'),
('GIAMGIA15%', 'F0000012', '2023-09-01', '2023-09-20'),
('GIAMGIA20%', 'F0000001', '2024-01-01', '2024-01-20'),
('GIAMGIA20%', 'F0000002', '2024-01-01', '2024-01-20'),
('GIAMGIA20%', 'F0000003', '2024-01-01', '2024-01-20'),
('GIAMGIA50%', 'F0000004', '2024-02-05', '2024-02-15'),
('GIAMGIA50%', 'F0000005', '2024-02-05', '2024-02-15'),
('GIAMGIA50%', 'F0000006', '2024-02-05', '2024-02-15'),
('GIAMGIA3%', 'F0000007', '2024-03-10', '2024-03-25'),
('GIAMGIA3%', 'F0000008', '2024-03-10', '2024-03-25'),
('GIAMGIA3%', 'F0000009', '2024-03-10', '2024-03-25'),
('GIAMGIA5%', 'F0000010', '2024-04-01', '2024-04-18'),
('GIAMGIA5%', 'F0000011', '2024-04-01', '2024-04-18'),
('GIAMGIA5%', 'F0000012', '2024-04-01', '2024-04-18'),
('GIAMGIA10%', 'F0000001', '2024-12-01', '2025-01-05'),
('GIAMGIA10%', 'F0000002', '2024-12-01', '2025-01-05'),
('GIAMGIA10%', 'F0000001', '2025-01-01', '2025-06-05'),
('GIAMGIA10%', 'F0000002', '2025-01-01', '2025-06-05'),
('GIAMGIA10%', 'F0000003', '2025-01-01', '2025-06-05');

--Nhập dữ liệu cho bảng customer order
GO

DECLARE @StartDate DATETIME = '2024-12-01 00:00:00';
DECLARE @EndDate DATETIME = '2025-01-09 23:59:59';
DECLARE @OrderIDPrefix CHAR(2) = 'OR';
DECLARE @OrderID INT = 1; -- Bắt đầu từ OR000001

WHILE @StartDate <= @EndDate
BEGIN
    -- Tạo ít nhất 1 đơn hàng mỗi ngày
    DECLARE @DailyOrders INT = 1;
    
    WHILE @DailyOrders <= 1
    BEGIN
        -- Sinh OrderID
        DECLARE @FormattedOrderID CHAR(8) = @OrderIDPrefix + RIGHT('000000' + CAST(@OrderID AS VARCHAR), 6);
        
        -- Random OrderedBy, AcceptedBy, ShippedBy
        DECLARE @OrderedBy VARCHAR(8) = 'KH' + RIGHT('000000' + CAST(ABS(CHECKSUM(NEWID())) % 20 + 1 AS VARCHAR), 6);
        DECLARE @AcceptedBy VARCHAR(8) = 'NV' + RIGHT('000000' + CAST(ABS(CHECKSUM(NEWID())) % 4 + 1 AS VARCHAR), 6);
        DECLARE @ShippedBy VARCHAR(8) = 'NV' + RIGHT('000000' + CAST(ABS(CHECKSUM(NEWID())) % 6 + 5 AS VARCHAR), 6);
        
        -- TotalPrice (192000 cho OrderID lẻ, 148000 cho OrderID chẵn)
        DECLARE @TotalPrice MONEY = CASE WHEN @OrderID % 2 = 1 THEN 192000 ELSE 148000 END;
        
        -- Tạo OrderDate ngẫu nhiên trong khoảng từ 08:00 đến 22:30
        DECLARE @OrderDate DATETIME = DATEADD(MINUTE, ABS(CHECKSUM(NEWID())) % 870, DATEADD(HOUR, 8, @StartDate)); -- 870 phút = 14.5 giờ (8h -> 22h30)
        DECLARE @AcceptDate DATETIME = DATEADD(MINUTE, 30, @OrderDate);
        DECLARE @ShipmentDate DATETIME = DATEADD(MINUTE, 30, @AcceptDate);
        DECLARE @FinishDate DATETIME = DATEADD(MINUTE, 30, @ShipmentDate);
        
        -- Insert dữ liệu
        INSERT INTO CustomerOrder (OrderID, OrderDate, AcceptDate, ShipmentDate, FinishDate, Status, OrderedBy, AcceptedBy, ShippedBy, TotalPrice)
        VALUES (@FormattedOrderID, @OrderDate, @AcceptDate, @ShipmentDate, @FinishDate, 3, @OrderedBy, @AcceptedBy, @ShippedBy, @TotalPrice);
        
        -- Tăng số đơn hàng và ID
        SET @OrderID = @OrderID + 1;
        SET @DailyOrders = @DailyOrders + 1;
    END

    -- Chuyển sang ngày tiếp theo
    SET @StartDate = DATEADD(DAY, 1, @StartDate);
END

GO
--Tạo dữ liệu cho bảng OrderDetail
DECLARE @OrderIDPrefix CHAR(2) = 'OR';
DECLARE @StartOrderID INT = 1;
DECLARE @EndOrderID INT = 40;

WHILE @StartOrderID <= @EndOrderID
BEGIN
    -- Sinh OrderID
    DECLARE @FormattedOrderID CHAR(8) = @OrderIDPrefix + RIGHT('000000' + CAST(@StartOrderID AS VARCHAR), 6);
    
    -- Nếu OrderID là lẻ
    IF @StartOrderID % 2 = 1
    BEGIN
        INSERT INTO OrderDetail (OrderID, FoodID, Amount)
        VALUES 
            (@FormattedOrderID, 'F0000010', 1),
            (@FormattedOrderID, 'F0000011', 1),
            (@FormattedOrderID, 'F0000012', 1);
    END
    -- Nếu OrderID là chẵn
    ELSE
    BEGIN
        INSERT INTO OrderDetail (OrderID, FoodID, Amount)
        VALUES 
            (@FormattedOrderID, 'F0000007', 1),
            (@FormattedOrderID, 'F0000008', 1);
    END

    -- Tăng OrderID
    SET @StartOrderID = @StartOrderID + 1;
END






 






