--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4
-- Dumped by pg_dump version 16.3

-- Started on 2024-08-20 16:06:42

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16435)
-- Name: logs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.logs (
    id integer NOT NULL,
    nivellog character varying(50) NOT NULL,
    mensagem text NOT NULL,
    excecao text,
    datacriacao timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.logs OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16434)
-- Name: logs_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.logs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.logs_id_seq OWNER TO postgres;

--
-- TOC entry 4824 (class 0 OID 0)
-- Dependencies: 217
-- Name: logs_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.logs_id_seq OWNED BY public.logs.id;


--
-- TOC entry 220 (class 1259 OID 16445)
-- Name: processamentomensagens; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.processamentomensagens (
    id integer NOT NULL,
    idmensagem uuid NOT NULL,
    dataprocessamento timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    status character varying(50) NOT NULL
);


ALTER TABLE public.processamentomensagens OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16444)
-- Name: processamentomensagens_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.processamentomensagens_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.processamentomensagens_id_seq OWNER TO postgres;

--
-- TOC entry 4825 (class 0 OID 0)
-- Dependencies: 219
-- Name: processamentomensagens_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.processamentomensagens_id_seq OWNED BY public.processamentomensagens.id;


--
-- TOC entry 216 (class 1259 OID 16426)
-- Name: tarefas; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tarefas (
    id integer NOT NULL,
    descricao character varying(255) NOT NULL,
    datavencimento date NOT NULL,
    status character varying(50) NOT NULL,
    datacriacao timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    dataatualizacao timestamp without time zone,
    usuarioid integer
);


ALTER TABLE public.tarefas OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 16425)
-- Name: tarefas_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tarefas_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tarefas_id_seq OWNER TO postgres;

--
-- TOC entry 4826 (class 0 OID 0)
-- Dependencies: 215
-- Name: tarefas_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tarefas_id_seq OWNED BY public.tarefas.id;


--
-- TOC entry 222 (class 1259 OID 16453)
-- Name: usuarios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.usuarios (
    id integer NOT NULL,
    nome character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    senha character varying(255) NOT NULL
);


ALTER TABLE public.usuarios OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16452)
-- Name: usuarios_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.usuarios_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.usuarios_id_seq OWNER TO postgres;

--
-- TOC entry 4827 (class 0 OID 0)
-- Dependencies: 221
-- Name: usuarios_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.usuarios_id_seq OWNED BY public.usuarios.id;


--
-- TOC entry 4651 (class 2604 OID 16438)
-- Name: logs id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs ALTER COLUMN id SET DEFAULT nextval('public.logs_id_seq'::regclass);


--
-- TOC entry 4653 (class 2604 OID 16448)
-- Name: processamentomensagens id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.processamentomensagens ALTER COLUMN id SET DEFAULT nextval('public.processamentomensagens_id_seq'::regclass);


--
-- TOC entry 4649 (class 2604 OID 16470)
-- Name: tarefas id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tarefas ALTER COLUMN id SET DEFAULT nextval('public.tarefas_id_seq'::regclass);


--
-- TOC entry 4655 (class 2604 OID 16481)
-- Name: usuarios id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios ALTER COLUMN id SET DEFAULT nextval('public.usuarios_id_seq'::regclass);


--
-- TOC entry 4814 (class 0 OID 16435)
-- Dependencies: 218
-- Data for Name: logs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.logs (id, nivellog, mensagem, excecao, datacriacao) FROM stdin;
\.


--
-- TOC entry 4816 (class 0 OID 16445)
-- Dependencies: 220
-- Data for Name: processamentomensagens; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.processamentomensagens (id, idmensagem, dataprocessamento, status) FROM stdin;
\.


--
-- TOC entry 4812 (class 0 OID 16426)
-- Dependencies: 216
-- Data for Name: tarefas; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tarefas (id, descricao, datavencimento, status, datacriacao, dataatualizacao, usuarioid) FROM stdin;
3	Teste update 1	2024-08-20	Pendente	2024-08-17 18:10:18.634852	\N	1
5	teste unico	2024-08-30	Cancelada	2024-08-17 18:42:01.848152	\N	1
8	Teste altercao para dto	2024-08-20	Pendente	2024-08-20 11:19:59.446733	\N	1
4	yuri teste 5	2024-08-21	Pendente	2024-08-17 18:10:34.125608	\N	1
11	teste para criar nova tarefa alterado	2024-08-20	Pendente	2024-08-20 15:33:58.352662	\N	1
12	teste de nova tarefa com o front	2024-08-30	Pendente	2024-08-20 15:34:48.182607	\N	1
\.


--
-- TOC entry 4818 (class 0 OID 16453)
-- Dependencies: 222
-- Data for Name: usuarios; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.usuarios (id, nome, email, senha) FROM stdin;
1	Yuri	yurigrandi@gmail.com	jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=
\.


--
-- TOC entry 4828 (class 0 OID 0)
-- Dependencies: 217
-- Name: logs_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.logs_id_seq', 1, false);


--
-- TOC entry 4829 (class 0 OID 0)
-- Dependencies: 219
-- Name: processamentomensagens_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.processamentomensagens_id_seq', 1, false);


--
-- TOC entry 4830 (class 0 OID 0)
-- Dependencies: 215
-- Name: tarefas_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tarefas_id_seq', 12, true);


--
-- TOC entry 4831 (class 0 OID 0)
-- Dependencies: 221
-- Name: usuarios_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.usuarios_id_seq', 2, true);


--
-- TOC entry 4660 (class 2606 OID 16443)
-- Name: logs logs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_pkey PRIMARY KEY (id);


--
-- TOC entry 4662 (class 2606 OID 16451)
-- Name: processamentomensagens processamentomensagens_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.processamentomensagens
    ADD CONSTRAINT processamentomensagens_pkey PRIMARY KEY (id);


--
-- TOC entry 4658 (class 2606 OID 16432)
-- Name: tarefas tarefas_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tarefas
    ADD CONSTRAINT tarefas_pkey PRIMARY KEY (id);


--
-- TOC entry 4664 (class 2606 OID 16460)
-- Name: usuarios usuarios_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_email_key UNIQUE (email);


--
-- TOC entry 4666 (class 2606 OID 16458)
-- Name: usuarios usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (id);


--
-- TOC entry 4656 (class 1259 OID 24576)
-- Name: ix_tarefas_status_datavencimento; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_tarefas_status_datavencimento ON public.tarefas USING btree (status, datavencimento);


--
-- TOC entry 4667 (class 2606 OID 16482)
-- Name: tarefas fk_usuario_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tarefas
    ADD CONSTRAINT fk_usuario_id FOREIGN KEY (usuarioid) REFERENCES public.usuarios(id);


-- Completed on 2024-08-20 16:06:42

--
-- PostgreSQL database dump complete
--

