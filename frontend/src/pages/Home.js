import React from 'react';
import hero from "../assets/icons/hero2.png"

function Home() {
  return (
    <>
      <div className='hero'>
        <div className='hero-circle'> </div>
        <img className='hero-img' src={hero} alt="" />
      </div>
      <div className='hero-about'>
        <div>
          <p>LIMITED OFFER</p>
          <h3> SAVE 10%</h3>
          <p>USE DISCOUNT</p>
          <button>10OFF</button>
        </div>
      </div>
      <div className='container'>
        <h1>Just arrived...</h1>
        {/* Shop brand */}
        <div className="bg-neutral-100 py-16">
          <div className="max-w-screen-xl px-4 mx-auto w-full grid grid-cols-1 gap-6 sm:grid-cols-2 md:grid-cols-4">
            <div to="/sneakers/nike">
              <img src="https://images.squarespace-cdn.com/content/v1/5e31b33a1b5911193c47e7b5/279b45e0-3243-4e58-aee2-dd18403e2085/nike-swoosh-logo.jpeg" alt="Nike" className="w-full object-cover sm:h-full" />
              <div className="mt-4">
                <a className="font-semibold uppercase" href="/sneakers/nike">Nike</a>
              </div>
            </div>
            <div to="/sneakers/adidas">
              <img src="https://cdn.britannica.com/94/193794-050-0FB7060D/Adidas-logo.jpg" alt="Adidas" className="w-full object-cover sm:h-full" />
              <div className="mt-4">
                <a className="font-semibold uppercase" href="/sneakers/adidas">Adidas</a>
              </div>
            </div>
            <div to="/sneakers/jordan">
              <img src="https://upload.wikimedia.org/wikipedia/en/thumb/3/37/Jumpman_logo.svg/1200px-Jumpman_logo.svg.png" alt="Jordan" className="w-full object-cover sm:h-full" />
              <div className="mt-4">
                <a className="font-semibold uppercase" href="/sneakers/jordan">Jordan</a>
              </div>
            </div>
            <div to="/sneakers/new-balance">
              <img src="https://logowik.com/content/uploads/images/new-balance.jpg" alt="New Balance" className="w-full object-cover sm:h-full" />
              <div className="mt-4">
                <a className="font-semibold uppercase" href="/sneakers/new-balance">New Balance</a>
              </div>
            </div>
          </div>
        </div>
        {/* Sign up */}
        <div className="relative overflow-hidden">
          <img src="https://migogkbh.dk/wp-content/uploads/2023/01/Skaermbillede-2023-01-26-kl.-10.12.43-1200x686.png" alt="" className="absolute h-full w-full object-cover brightness-50" />
          <div className="max-w-screen-xl px-4 mx-auto w-full relative flex flex-col py-24 md:flex-row md:justify-between">
            <div>
              <h2 className="text-3xl font-semibold text-white">Subscribe to our newsletter</h2>
              <p className="mt-4 text-lg text-gray-200">Get the latest news and updates</p>
            </div>
            <div className="mt-6 flex overflow-hidden rounded-md">
              <input type="email" placeholder="Enter your email" className="h-12 w-full border-none bg-black/50 px-4 text-white focus:outline-none" />
              <button className="h-12 whitespace-nowrap bg-black px-4 text-white">Sign Up</button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default Home;