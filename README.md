create table account (
id int primary key auto_increment,
username varchar(30) not null,
password varchar(30) not null,
isAuthor int not null,
);

create table book (
isbn int primary key,
title varchar(30) not null,
pages int not null,
genre varchar(30) not null,
publishingdate date not null
);
