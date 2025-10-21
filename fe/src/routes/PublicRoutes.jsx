import React from "react";
import { Routes, Route } from "react-router-dom";
import Home from "../pages/Home";
import Services from "../pages/Services";
import Equipment from "../pages/Equipment";
import Contact from "../pages/Contact";

const PublicRoutes = () => {
  return (
    <>
      <Home />
    </>
  );
};

export default PublicRoutes;
