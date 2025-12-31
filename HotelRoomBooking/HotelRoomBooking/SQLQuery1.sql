-- Veritabanı oluşturma (eğer yoksa)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HotelRoomBooking1')
BEGIN
    CREATE DATABASE HotelRoomBooking1;
END
GO

USE HotelRoomBooking1;
GO

-- Eğer tablo varsa sil (isteğe bağlı - sadece test için)
-- DROP TABLE IF EXISTS Bookings;
-- DROP TABLE IF EXISTS customers;
-- DROP TABLE IF EXISTS Room;
-- DROP TABLE IF EXISTS RoomType;
-- DROP TABLE IF EXISTS users;
-- GO

Create table users
(
	userID int primary key identity,
	uName varchar(50) not null,	
	uUsername varchar(50) not null,	
	uPass varchar(50) not null,	
	uPhone varchar(50) 	
);

Create table RoomType
(
	typeID int primary key identity,
	tName varchar(50) not null,
	tDescription varchar(50)
);

create table Room
(
	roomID int primary key identity,
	rName varchar(50) not null,
	rTypeID int, 
	rRate float,
	rStatus varchar(50) 
);

create table customers
(
	cusID int primary key identity,
	cName varchar(50) not null,
	cPhone varchar(50),
	cEmail varchar(50)
);


create table Bookings
(
    bookID int primary key identity,
    customerID int not null,
    roomID int not null,
    CheckInDate date,
    bCheckOutDate date,
    Status varchar(50),
    days int,
    rate float,
    amount float,
    received float,
    [change] float   -- köşeli parantez ekledik
);

  



select 
 b.bookID,
 b.customerID,
 c.cName,
 b.roomID,
 r.rName,
 b.CheckInDate,
 b.bCheckOutDate,
 b.days,
 b.rate,
 b.amount,
 b.received,
 b.change,
 b.status
from Bookings b
inner join customers c on c.cusID = b.customerID
inner join Room r on r.roomID = b.roomID

-- Varsayılan kullanıcı ekleme (Default User)
-- Kullanıcı Adı: admin
-- Şifre: admin
INSERT INTO users (uName, uUsername, uPass, uPhone) 
VALUES ('Admin', 'admin', 'admin', '1234567890'); 
			   


