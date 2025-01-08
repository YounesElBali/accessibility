import React, { useState } from 'react';
import { Button, Form, Alert } from 'react-bootstrap';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Access from './Access';
//import { useGoogleLogin } from "@react-oauth/google";
const Login = () => {
 // const [google, setGoogle] = useState();
  const history = useNavigate();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);

  const handleUserNameChange = (event) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
  
    const backendEndpoint = process.env.REACT_APP_API_URL +'/login';
  
    const formData = {
      UserName: username,
      Password: password,
    };
  
    try {
      const response = await axios.post(backendEndpoint, formData);
      const token = response.data;
      
      localStorage.setItem("token", token);
      localStorage.setItem("username", username);
      await Access();
      redirect();
      window.location.reload();
    } catch (error) {
      console.error('Error submitting form to backend:', error);
  
      if (error.response) {
          setError("Invalid gebruikersnaam of wachtwoord");
      }
    }
  };
  
// const login = useGoogleLogin({
//   onSuccess: async (response) => {
//     try {
//       const data = await axios.get(
//         "https://www.googleapis.com/oauth2/v1/userinfo",
//         {
//           headers: {
//             Authorization: `Bearer ${response.access_token}`,
//           },
//         }
//       );
//       sessionStorage.setItem("authenticated", false);
//       sessionStorage.setItem("role", "ervaringsDeskundige");

//       const googleAcountData = data.data;

//       setGoogle(googleAcountData);

//       sessionStorage.setItem(
//         "googleAccount",
//         JSON.stringify(googleAcountData)
//       );
//     } catch (err) {
//       console.log(err);
//     }
//   },
// });


  const redirect = () => {
    const userRole = localStorage.getItem("role");
    switch (userRole) {
      case 'Expert':
        history('/expertHome');
        break;
      case 'Business':
        history('/businessHome');
        break;
      case 'Admin':
        history('/adminHome');
        break;
      default:
        history('/');
    }
  };
  return (
    <Form onSubmit={handleSubmit}>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form.Group className="mb-3" controlId="formBasicUserName">
        <h1>Login</h1>
        <Form.Label>Vul je gebruikersnaam in</Form.Label>
        <Form.Control type="Username" value={username} onChange={handleUserNameChange} placeholder="vul in gebruikersnaam"/>
      </Form.Group>

      <Form.Group className="mb-3" controlId="formBasicPassword">
        <Form.Label>Vul je wachtwoord in</Form.Label>
        <Form.Control type="password" value={password}  onChange={handlePasswordChange} placeholder="vul in wachtwoord" />
      </Form.Group>

      <Button variant="primary" type="submit">
        Inloggen
      </Button>
    </Form>
  );
};

export default Login;



