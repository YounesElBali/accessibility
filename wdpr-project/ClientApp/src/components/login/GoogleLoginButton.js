// GoogleLoginButton.js
import React from 'react';
import { GoogleLogin } from '@react-oauth/google';

const GoogleLoginButton = ({ onLoginSuccess, onLoginFailure }) => {
  const clientId = "1012765062649-rchsk8hcpet6055qpdbm7opu4t4nvefm.apps.googleusercontent.com"; // Replace with your actual Google Client ID

  return (
    <GoogleLogin
      clientId={clientId}
      buttonText="Login with Google"
      onSuccess={onLoginSuccess}
      onFailure={onLoginFailure}
      cookiePolicy={'single_host_origin'}
    />
  );
};

export default GoogleLoginButton;
