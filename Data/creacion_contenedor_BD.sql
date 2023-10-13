#Instrucciones para general la base de datos a traves de un contenedor en Docker y su configuracion

--Lanzar contenedor con postgres
docker run --name buses_parcial3 -e POSTGRES_PASSWORD=1234 -d -p 5432:5432 postgres:latest

--Creacion de un superusuario 
--Configurar la base de datos y los permisos
create database buses_db;
\c buses_db;
create schema core;

--Crear usuario para manejar la app

create user buses_app with encrypted password '1234';
grant connect on database buses_db to buses_app;
grant create on database buses_db to buses_app;
grant create, usage on schema core to buses_app;
alter user buses_app set search_path to core;
set timezone='Ameriza/Bogota';

--crear tablas
--horas
create table core.horas
(
    id integer not null primary key,
    eshorapico boolean not null
)
create table core.cargadores
(
    id integer generated always as identity constraint cargadores_pk primary key,
    marca varchar(20) not null
)
create table core.buses
(
    id integer generated always as identity constraint buses_pk primary key,
    ruta varchar(30) not null
)
create table core.cargadores_utilizados
(
    id integer generated always as identity constraint cargadores_utilizados_pk primary key,
    hora_id integer not null constraint cargadores_utilizados_horas_fk references core.horas,
    cargador_id integer not null constraint cargadores_utilizados_cargadores_fk references core.cargadores,
    bus_id integer not null constraint cargadores_utilizados_buses_fk references core.buses
)
create table core.operaciones
(
    id integer generated always as identity constraint operaciones_pk primary key,
    hora_id integer not null constraint operaciones_horas_fk references core.horas,
    bus_id integer not null constraint operaciones_buses_fk references core.buses
)

--como subir los datos
--1. horas
--2. cargadores
--3. buses

--Creacion de usuario con acceso a consultas CRUD

create user buses_usr with encrypted password '1234';
grant connect on database buses_db to buses_usr;
grant usage on schema core to buses_usr;
grant select, insert, update, delete on all tables in schema core to buses_usr;
grant execute on all routines in schema core to buses_usr;
alter user buses_usr set search_path to core;

-- -------------------------------------
-- Procedimientos asociados al cargador
-- -------------------------------------

-- Inserción
create or replace procedure core.p_inserta_cargador(
                    in p_marca varchar)
    language plpgsql
as
$$
    begin
        insert into cargadores (marca)
        values (p_marca);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_cargador(
                    in p_id integer,
                    in p_marca varchar)
    language plpgsql
as
$$
    begin
        update cargadores
        set
            marca              = p_marca
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_cargador(in p_id integer)
language plpgsql as
$$
    begin
        delete from cargadores c where c.id = p_id;
    end;
$$;

-- -------------------------------------
-- Procedimientos asociados al bus
-- -------------------------------------
-- Inserción
create or replace procedure core.p_inserta_bus(
                    in p_ruta varchar)
    language plpgsql
as
$$
    begin
        insert into buses (ruta)
        values (p_ruta);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_bus(
                    in p_id integer,
                    in p_ruta varchar)
    language plpgsql
as
$$
    begin
        update buses
        set
            ruta = p_ruta
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_bus(in p_id integer)
language plpgsql as
$$
    begin
        delete from buses b where b.id = p_id;
    end;
$$;
