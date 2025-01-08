import { jwtDecode } from 'jwt-decode';

export default function Access() {
  try {
    let token = localStorage.getItem("token");
    const decoded = jwtDecode(token);
    const userName = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    const userRole = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    
    if (userRole !== "Expert" && userRole !== "Admin" && userRole !== "Business") {
      localStorage.setItem("toegang", false);
    }
    localStorage.setItem("toegang", true);
    localStorage.setItem("userName", userName);
    localStorage.setItem("userId", userId);
    localStorage.setItem("role", userRole);
  } catch {
    localStorage.setItem("toegang", false);
  }
}
