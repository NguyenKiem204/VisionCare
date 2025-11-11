import React, { useState, useEffect } from 'react';
import { Eye, Award, Heart, Clock, ChevronRight, Play, X } from 'lucide-react';

const Services = () => {
  const [scrolled, setScrolled] = useState(false);
  const [activeTestimonial, setActiveTestimonial] = useState(0);
  const [selectedService, setSelectedService] = useState(null);

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 50);
    };
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  useEffect(() => {
    const interval = setInterval(() => {
      setActiveTestimonial((prev) => (prev + 1) % testimonials.length);
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  const services = [
    {
      title: 'Comprehensive Eye Exams',
      description: 'Thorough evaluation of your vision and eye health with advanced diagnostic technology.',
      icon: Eye,
      details: 'Our comprehensive exams include visual acuity testing, refraction, eye pressure measurement, and retinal imaging to detect early signs of conditions like glaucoma and macular degeneration.'
    },
    {
      title: 'Cataract Surgery',
      description: 'Advanced, minimally invasive cataract procedures with premium lens options.',
      icon: Eye,
      details: 'We use the latest phacoemulsification techniques and offer premium intraocular lenses including multifocal and toric options for optimal vision correction.'
    },
    {
      title: 'LASIK & Refractive Surgery',
      description: 'Blade-free laser vision correction for lasting freedom from glasses and contacts.',
      icon: Eye,
      details: 'Our all-laser LASIK procedure uses wavefront-guided technology for personalized treatment and exceptional visual outcomes.'
    },
    {
      title: 'Pediatric Eye Care',
      description: 'Specialized care for childrens developing vision, from infancy through adolescence',
      icon: Heart,
      details: 'Early detection and treatment of amblyopia, strabismus, and refractive errors in a child-friendly environment.'
    },
    {
      title: 'Glaucoma Treatment',
      description: 'Comprehensive management to preserve your vision and prevent progression.',
      icon: Award,
      details: 'From advanced drops to laser therapy and minimally invasive surgical options, we tailor treatment to your specific needs.'
    },
    {
      title: 'Contact Lens Fitting',
      description: 'Expert fitting services for all lens types, including specialty and complex prescriptions.',
      icon: Eye,
      details: 'Comprehensive fitting for daily, extended wear, toric, multifocal, and specialty lenses like scleral and orthokeratology.'
    }
  ];

  const testimonials = [
    {
      name: 'Sarah Thompson',
      age: '42',
      quote: 'After my cataract surgery, I can see colors I forgot existed. The care team made me feel safe every step of the way.',
      image: 'ST'
    },
    {
      name: 'Michael Chen',
      age: '35',
      quote: 'LASIK changed my life. No more glasses fogging up or contacts drying out. The procedure was quick and painless.',
      image: 'MC'
    },
    {
      name: 'Emily Rodriguez',
      age: '28',
      quote: 'The pediatric team was wonderful with my daughter. They detected her vision issues early, and now shes thriving in school.',
      image: 'ER'
    }
  ];

  const steps = [
    { number: '01', title: 'Schedule Consultation', description: 'Book your appointment online or call our care team' },
    { number: '02', title: 'Comprehensive Exam', description: 'Receive a thorough evaluation by our specialists' },
    { number: '03', title: 'Personalized Care', description: 'Get a treatment plan tailored to your needs' }
  ];

  return (
    <div className="min-h-screen bg-white font-sans">

      {/* Hero Section */}
      <section className="relative h-[45vh] min-h-[450px] overflow-hidden">
        <div className="absolute inset-0 bg-gradient-to-r from-gray-900/70 to-gray-900/30 z-10"></div>
        <img
          src="https://images.unsplash.com/photo-1631815588090-d4bfec5b1ccb?w=1920&q=80"
          alt="Eye examination"
          className="absolute inset-0 w-full h-full object-cover"
        />
        <div className="relative z-20 max-w-7xl mx-auto px-6 h-full flex items-center">
          <div className="max-w-2xl text-white">
            <h1 className="text-5xl md:text-6xl font-light mb-6 leading-tight">
              Your Vision,<br />
              <span className="font-semibold">Our Priority</span>
            </h1>
            <p className="text-xl md:text-2xl mb-10 text-gray-200 font-light leading-relaxed">
              Comprehensive eye care designed around your needs, delivered with expertise and compassion.
            </p>
            <div className="flex flex-wrap gap-4">
              <button className="bg-blue-600 hover:bg-blue-700 text-white px-8 py-4 rounded-full text-base font-medium transition-all">
                Book Appointment
              </button>
              <button className="bg-transparent border-2 border-white hover:bg-white hover:text-gray-900 text-white px-8 py-4 rounded-full text-base font-medium transition-all">
                Explore Services
              </button>
            </div>
          </div>
        </div>
      </section>

      {/* Services Grid */}
      <section className="py-24 bg-gray-50">
        <div className="max-w-7xl mx-auto px-6">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-light text-gray-900 mb-4">
              Our <span className="font-semibold">Services</span>
            </h2>
            <p className="text-xl text-gray-600 max-w-2xl mx-auto">
              From routine exams to advanced surgical procedures, we provide comprehensive care for all your vision needs
            </p>
          </div>

          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
            {services.map((service, index) => (
              <div
                key={index}
                onClick={() => setSelectedService(service)}
                className="bg-white rounded-2xl p-8 shadow-sm hover:shadow-xl transition-all duration-300 cursor-pointer group border border-gray-100"
              >
                <div className="w-14 h-14 bg-blue-50 rounded-xl flex items-center justify-center mb-6 group-hover:bg-blue-600 transition-colors">
                  <service.icon className="w-7 h-7 text-blue-600 group-hover:text-white transition-colors" />
                </div>
                <h3 className="text-xl font-semibold text-gray-900 mb-3">
                  {service.title}
                </h3>
                <p className="text-gray-600 mb-4 leading-relaxed">
                  {service.description}
                </p>
                <div className="flex items-center text-blue-600 font-medium group-hover:gap-2 transition-all">
                  Learn More
                  <ChevronRight className="w-5 h-5 ml-1 group-hover:translate-x-1 transition-transform" />
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Featured Service */}
      <section className="py-24">
        <div className="max-w-7xl mx-auto px-6">
          <div className="grid md:grid-cols-2 gap-16 items-center">
            <div className="relative h-[500px] rounded-3xl overflow-hidden">
              <img
                src="https://images.unsplash.com/photo-1581594693702-fbdc51b2763b?w=800&q=80"
                alt="LASIK procedure"
                className="w-full h-full object-cover"
              />
            </div>
            <div>
              <h2 className="text-4xl md:text-5xl font-light text-gray-900 mb-6">
                Advanced <span className="font-semibold">LASIK Technology</span>
              </h2>
              <p className="text-lg text-gray-600 mb-6 leading-relaxed">
                Experience the freedom of clear vision with our blade-free, all-laser LASIK procedure. Using the most advanced wavefront-guided technology, we create a personalized treatment map unique to your eyes.
              </p>
              <p className="text-lg text-gray-600 mb-8 leading-relaxed">
                Most patients achieve 20/20 vision or better and return to normal activities within 24 hours. Our experienced surgeons have performed over 10,000 successful procedures.
              </p>
              <button className="text-blue-600 hover:text-blue-700 font-semibold text-lg flex items-center group">
                Discover More
                <ChevronRight className="w-5 h-5 ml-1 group-hover:translate-x-1 transition-transform" />
              </button>
            </div>
          </div>
        </div>
      </section>

      {/* How It Works */}
      <section className="py-24 bg-gradient-to-b from-blue-50 to-white">
        <div className="max-w-7xl mx-auto px-6">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-light text-gray-900 mb-4">
              Your <span className="font-semibold">Journey</span> With Us
            </h2>
            <p className="text-xl text-gray-600 max-w-2xl mx-auto">
              A seamless experience from your first visit to ongoing care
            </p>
          </div>

          <div className="grid md:grid-cols-3 gap-12 relative">
            {steps.map((step, index) => (
              <div key={index} className="relative">
                <div className="bg-white rounded-2xl p-8 shadow-sm h-full">
                  <div className="text-6xl font-light text-blue-100 mb-4">{step.number}</div>
                  <h3 className="text-2xl font-semibold text-gray-900 mb-3">
                    {step.title}
                  </h3>
                  <p className="text-gray-600 leading-relaxed">
                    {step.description}
                  </p>
                </div>
                {index < steps.length - 1 && (
                  <div className="hidden md:block absolute top-16 -right-6 w-12 h-0.5 bg-blue-200"></div>
                )}
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Technology & Care */}
      <section className="py-24">
        <div className="max-w-7xl mx-auto px-6">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-light text-gray-900 mb-4">
              State-of-the-Art <span className="font-semibold">Technology</span>
            </h2>
            <p className="text-xl text-gray-600 max-w-2xl mx-auto">
              Precision equipment and comfortable facilities for your care
            </p>
          </div>

          <div className="grid md:grid-cols-3 gap-8">
            <div className="relative h-80 rounded-2xl overflow-hidden group">
              <img
                src="https://images.unsplash.com/photo-1579684385127-1ef15d508118?w=600&q=80"
                alt="Laser imaging"
                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
              />
              <div className="absolute inset-0 bg-gradient-to-t from-gray-900/80 to-transparent flex items-end">
                <p className="text-white text-lg font-medium p-6">Precision Laser Imaging</p>
              </div>
            </div>
            <div className="relative h-80 rounded-2xl overflow-hidden group">
              <img
                src="https://images.unsplash.com/photo-1519494026892-80bbd2d6fd0d?w=600&q=80"
                alt="Examination room"
                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
              />
              <div className="absolute inset-0 bg-gradient-to-t from-gray-900/80 to-transparent flex items-end">
                <p className="text-white text-lg font-medium p-6">Modern Examination Rooms</p>
              </div>
            </div>
            <div className="relative h-80 rounded-2xl overflow-hidden group">
              <img
                src="https://images.unsplash.com/photo-1538108149393-fbbd81895907?w=600&q=80"
                alt="Recovery room"
                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
              />
              <div className="absolute inset-0 bg-gradient-to-t from-gray-900/80 to-transparent flex items-end">
                <p className="text-white text-lg font-medium p-6">Comfort Recovery Suite</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Testimonials */}
      <section className="py-24 bg-gray-50">
        <div className="max-w-5xl mx-auto px-6">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-light text-gray-900 mb-4">
              Patient <span className="font-semibold">Stories</span>
            </h2>
          </div>

          <div className="relative bg-white rounded-3xl shadow-lg p-12 md:p-16">
            <div className="text-center">
              <div className="w-20 h-20 bg-blue-100 rounded-full flex items-center justify-center text-2xl font-semibold text-blue-600 mx-auto mb-6">
                {testimonials[activeTestimonial].image}
              </div>
              <p className="text-2xl text-gray-700 mb-8 leading-relaxed italic">
                "{testimonials[activeTestimonial].quote}"
              </p>
              <p className="text-lg font-semibold text-gray-900">
                {testimonials[activeTestimonial].name}
              </p>
              <p className="text-gray-600">Age {testimonials[activeTestimonial].age}</p>
            </div>

            <div className="flex justify-center gap-2 mt-12">
              {testimonials.map((_, index) => (
                <button
                  key={index}
                  onClick={() => setActiveTestimonial(index)}
                  className={`w-3 h-3 rounded-full transition-all ${
                    index === activeTestimonial ? 'bg-blue-600 w-8' : 'bg-gray-300'
                  }`}
                />
              ))}
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      {/* <section className="py-24 bg-gradient-to-br from-blue-600 to-blue-700">
        <div className="max-w-4xl mx-auto px-6 text-center text-white">
          <h2 className="text-4xl md:text-5xl font-light mb-6">
            See the world more clearly with VisionCare
          </h2>
          <p className="text-xl mb-10 text-blue-100">
            Book your comprehensive eye exam today and take the first step toward better vision
          </p>
          <button className="bg-white hover:bg-gray-100 text-blue-600 px-10 py-4 rounded-full text-lg font-semibold transition-colors shadow-lg">
            Book Appointment Now
          </button>
        </div>
      </section> */}

      {/* Service Modal */}
      {selectedService && (
        <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-4" onClick={() => setSelectedService(null)}>
          <div className="bg-white rounded-3xl max-w-2xl w-full p-8 md:p-12 relative" onClick={(e) => e.stopPropagation()}>
            <button
              onClick={() => setSelectedService(null)}
              className="absolute top-6 right-6 text-gray-400 hover:text-gray-600"
            >
              <X className="w-6 h-6" />
            </button>
            <div className="w-16 h-16 bg-blue-50 rounded-2xl flex items-center justify-center mb-6">
              <selectedService.icon className="w-8 h-8 text-blue-600" />
            </div>
            <h3 className="text-3xl font-semibold text-gray-900 mb-4">
              {selectedService.title}
            </h3>
            <p className="text-lg text-gray-600 mb-6 leading-relaxed">
              {selectedService.description}
            </p>
            <p className="text-base text-gray-600 leading-relaxed mb-8">
              {selectedService.details}
            </p>
            <button className="bg-blue-600 hover:bg-blue-700 text-white px-8 py-3 rounded-full font-medium transition-colors w-full">
              Schedule Consultation
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default Services;