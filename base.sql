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

create table travaux(
    idTrav AS ('trav' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    num varchar(50),
    designation varchar(100)
);
insert into travaux (num, designation)
values
    ('000', 'TRAVAUX PREPARATIORE'),
    ('100', 'TRAVAUX DE TERRASSEMENT'),
    ('200', 'TRAVAUX EN INFRASTRUCTURE');

create table tache(
    idTache AS ('tache' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    num varchar(50),
    designation varchar(100),
    unite varchar(10),
    pu float,
    idTrav varchar(14) references travaux(idTrav)
);
insert into tache (num, designation, unite, pu, idTrav)
values
    ('001', 'Mur de soutenement et demi Cloture ht 1m', 'm3', 190000.00, 'trav1');
insert into tache (num, designation, unite, pu, idTrav)
values
    ('101', 'Décapage des terrains meubles', 'm2', 3072.87, 'trav2'),
    ('102', 'Dressage du plateforme', 'm2', 3736.26, 'trav2'),
    ('103', 'Fouille d''ouvrage terrain ferme', 'm3', 9390.93, 'trav2'),
    ('104', 'Remblai d''ouvrage', 'm3', 37563.26, 'trav2'),
    ('105', 'Travaux d''implantation', 'fft', 152656.00, 'trav2');
insert into tache (num, designation, unite, pu, idTrav)
values
    ('201', 'Maçonnerie de moellons, ep= 35cm', 'm3', 172114.40, 'trav3'),
    ('202', 'Beton armée dosée à 350kg/m3 - semelles isolée', 'm3', 573215.80, 'trav3'),
    ('202', 'Beton armée dosée à 350kg/m3 - amorces poteaux', 'm3', 573215.80, 'trav3'),
    ('202', 'Beton armée dosée à 350kg/m3 - chaînage bas de 20x20', 'm3', 573215.80, 'trav3'),
    ('203', 'Remblai technique', 'm3', 37563.26, 'trav3'),
    ('204', 'Herrissonage ep=10', 'm3', 73245.40, 'trav3'),
    ('205', 'Beton ordinaire dosée à 300kg/m3 pour for', 'm3', 487815.80, 'trav3'),
    ('205', 'Chape de 2cm', 'm3', 33566.4, 'trav3');

create table devis(
    idDevis AS ('devis' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    nomClient varchar(100),
    dateDemande datetime
);

create table detailDevis(
    idDetail AS ('detail' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    idDevis varchar(10) references devis(idDevis),
    idTache varchar(10) references tache(idTache),
    quantite int
    pu int
);

