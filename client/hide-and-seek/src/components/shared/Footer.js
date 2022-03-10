import React from "react";

export const Footer = () => {
  const year = new Date().getFullYear();

  return (
    <footer className="footer mt-auto navbar navbar-expand-lg bg-light">
      <div className="container justify-content-center">
        <span className="navbar-brand app-logo">&copy; {year} - HideAndSeek</span>
      </div>
    </footer>
  );
};
