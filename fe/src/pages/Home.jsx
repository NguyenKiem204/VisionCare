import React from "react";
import HeroSlider from "../components/home/HeroSlider";
import StatsBar from "../components/home/StatsBar";
import AboutSection from "../components/home/AboutSection";
import ServicesGrid from "../components/home/ServicesGrid";
import DoctorsCarousel from "../components/home/DoctorsCarousel";
import Testimonials from "../components/home/Testimonials";
import NewsSection from "../components/home/NewsSection";
import CallToAction from "../components/home/CallToAction";

const Home = () => {
  return (
    <div className="relative">
    <div aria-hidden className="pointer-events-none fixed inset-0 -z-10">
      <div className="absolute inset-0 bg-[url('https://plus.unsplash.com/premium_photo-1677333508737-6b6d642bc6d6?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D')] bg-center bg-cover bg-fixed" />
    </div>

    <div className="relative z-0">
      <HeroSlider />
      <AboutSection />
      <StatsBar />
      <DoctorsCarousel />
      <ServicesGrid />
      <Testimonials />
      <NewsSection />
      <CallToAction />
    </div>
  </div>
  );
};

export default Home;
