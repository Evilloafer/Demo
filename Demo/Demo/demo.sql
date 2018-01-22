create table userInfo
(
	id int identity(1,1) primary key not null,
	name nvarchar(20),
	pwd nvarchar(20)
)

insert into userInfo(name,pwd) values('admin','111');
insert into userInfo(name,pwd) values('张三','111');

select * from userInfo;
