import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/common/Header";
import Footer from "./components/common/Footer";
import Home from "./pages/Home";
import Services from "./pages/Services";
import Equipment from "./pages/Equipment";
import Booking from "./pages/Booking";
import "./index.css";

function App() {
  return (
    <Router>
      <div className="min-h-screen">
        <Header />
        <main>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/services" element={<Services />} />
            <Route path="/equipment" element={<Equipment />} />
            <Route path="/booking" element={<Booking />} />
            <Route
              path="/contact"
              element={<div className="pt-20">Contact Page</div>}
            />
          </Routes>
        </main>
        <Footer />
      </div>
    </Router>
  );
}

export default App;
