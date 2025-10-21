import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";
import { initAuthAutoRefresh } from "./utils/api";

// Initialize background auth refresh (visibility + interval)
initAuthAutoRefresh();

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
