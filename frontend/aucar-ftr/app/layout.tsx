import type { Metadata } from "next";
import { Poppins, Montserrat } from "next/font/google";
import "./globals.css";
import Navbar from "./nav/Navbar";

const poppins = Poppins({ subsets: ["latin"], weight: ["300", "400", "600"] });
const montserrat = Montserrat({ subsets: ["latin"], weight: ["300", "400", "600"] });

export const metadata: Metadata = {
  title: "Aucar | Home",
  description: "Bid for your dream car at Aucar.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className={poppins + " " + montserrat}>
        <Navbar />
        <main className='container mx-auto px-5 pt-10'>
          {children}
        </main>
      </body>
    </html>
  );
}
