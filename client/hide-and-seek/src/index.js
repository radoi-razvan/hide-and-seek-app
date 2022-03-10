import "./css/App.css";
import React, { StrictMode } from "react";
import { render } from "react-dom";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import { Header } from "./components/shared/Header";
import { Footer } from "./components/shared/Footer";
import { Home } from "./components/shared/Home";
import { NotFound } from "./components/shared/NotFound";
import { Cryptography } from "./components/Cryptography";

const App = () => {
  return (
    <>
      <Header />
      <Routes>
        <Route path="/" element={<Home />}></Route>
        <Route path="/cryptography" element={<Cryptography />}></Route>
        <Route path="*" element={<NotFound />}></Route>
      </Routes>
      <Footer />
    </>
  );
};

render(
  <StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StrictMode>,
  document.getElementById("root")
);
