create table temperature
(
    time varchar(255)   not null
        constraint temperature_pkey
            primary key,
    temp numeric(10, 2) not null
);

drop table temperature;

