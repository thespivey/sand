import express from 'express';
import cors from 'cors';

var app = express();
app.use(cors());
app.use(express.static("../patterns"));
app.listen(8080);