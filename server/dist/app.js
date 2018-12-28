"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const body_parser_1 = __importDefault(require("body-parser"));
const morgan_1 = __importDefault(require("morgan"));
const cookie_parser_1 = __importDefault(require("cookie-parser"));
require("reflect-metadata");
const index_1 = __importDefault(require("./controllers/index"));
// Create Express server
const app = express_1.default();
app.set("port", process.env.PORT || 1337);
app.use(morgan_1.default('dev'));
app.use(body_parser_1.default.json());
app.use(express_1.default.urlencoded({ extended: false }));
app.use(cookie_parser_1.default());
app.use('/', index_1.default);
exports.default = app;
//# sourceMappingURL=app.js.map