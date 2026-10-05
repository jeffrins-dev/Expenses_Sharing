import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Expense Sharing",
  description: "Expense sharing application",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}