import express from "express";
import bodyParser from "body-parser";
import logger from 'morgan';
import cookieParser from 'cookie-parser';
import 'reflect-metadata';
import indexRouter from './controllers/index';

// Create Express server
const app = express();

app.set("port", process.env.PORT || 1337);

app.use(logger('dev'));
app.use(bodyParser.json());
app.use(express.urlencoded({extended: false}));
app.use(cookieParser());

app.use('/', indexRouter);
  
export default app;
