create database btp;

create table uuser(
    idUser AS ('u' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    name varchar(100),
    passWord varchar(150),
    admin int --0:no, 1:yes
);
insert into uuser (name, passWord, admin)
values
    ('Rakoto', '123', 0),
    ('Rabe', '123', 0),
    ('Admin', 'admin', 1);