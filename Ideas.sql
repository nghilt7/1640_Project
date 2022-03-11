create database Ideas
go

use Ideas
go

create table Categories(
CategoryID int primary key identity(1,1),
CategoryName nvarchar(max),
CategoryDescription nvarchar(max))
go

create table Departments(
DepartmentID int primary key identity(1,1),
DepartmentName nvarchar(max))
go

create table Files(
FileID int primary key identity(1,1),
FileUrl nvarchar(max),
FileName nvarchar(max),
UploadDate datetime,
LastChange datetime
)
go

create table Roles(
RoleID int primary key identity(1,1),
RoleName nvarchar(max))
go

create table Submissions(
SubmissionID int primary key identity(1,1),
SubmissionName nvarchar(max),
SubmissionDescription nvarchar(max),
CloseDate datetime,
FinalDate datetime,
CategoryID int references Categories(CategoryID) on delete cascade
)
go

create table Users(
UserID int primary key identity(1,1),
Name nvarchar(max),
Email nvarchar(max),
PasswordHash nvarchar(max),
PhoneNumber nvarchar(max),
DepartmentID int references Departments(DepartmentID) on delete cascade,
RoleID int references Roles(RoleID) on delete cascade)
go

create table Ideas(
IdeaID int primary key identity(1,1),
Title nvarchar(max),
Description nvarchar(max),
Content nvarchar(max),
CreateDate datetime,
ViewCount int,
UserID int references Users(UserID) on delete cascade,
CategoryID int references Categories(CategoryID) on delete cascade,
SubmissionID int references Submissions(SubmissionID))
go

create table Comments(
CommentID int primary key identity(1,1),
Content nvarchar(max),
CommentDate datetime,
IdeaID int references Ideas(IdeaID) on delete cascade,
UserID int references Users(UserID),
VotesCount int)
go

create table Votes(
VoteID int primary key identity(1,1),
UserID int references Users(UserID),
CommentID int references Comments(CommentID) on delete cascade,
VoteValue int)
go

use Ideas
go

insert into Departments values('IT')
go

insert into Roles values('Admin')
go
insert into Roles values('Staff')
go
insert into Roles values('QAM')
go
insert into Roles values('QAC')
go

insert into Users values('admin', 'admin@gmail.com', '123456', '0123456789', 1, 1)
go
insert into Users values('staff', 'staff@gmail.com', '123456', '0123456789', 1, 2)
go
insert into Users values('QAM', 'qam@gmail.com', '123456', '0123456789', 1, 3)
go
insert into Users values('QAC', 'qac@gmail.com', '123456', '0123456789', 1, 4)
go

insert into Categories values('Technology', 'Talk about tachnology')
go
insert into Categories values('Information', 'Talk about information')
go
insert into Categories values('Design', 'Talk about design')
go

insert into Submissions values('Deadline IT', 'Deadline', '3-7-2022', '3-10-2022', '1')
go
insert into Submissions values('Deadline Information', 'Deadline', '3-7-2022', '3-10-2022', '2')
go
insert into Submissions values('Deadline Design', 'Deadline', '3-7-2022', '3-10-2022', '3')
go
