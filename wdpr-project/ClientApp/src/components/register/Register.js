import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios from 'axios';

const Register = () => {
  const [formData, setFormData] = useState({
    Name: '',
    UserName: '',
    password: '',
    Address: {
      Adress: '',
      PostCode: '',
    },
    URL: ''
  });

    const handleInputChange = (field, value) => {
    
      // Use reduce to traverse the nested structure
      setFormData((prevData) => {
        const nestedFields = field.split('.');
        const updatedData = { ...prevData };
        let currentField = updatedData;
    
        for (let i = 0; i < nestedFields.length - 1; i++) {
          currentField = currentField[nestedFields[i]];
        }
    
        currentField[nestedFields[nestedFields.length - 1]] = value;
    
        return updatedData;
      });
    };

  const submit = () => {
    const backendEndpoint = process.env.REACT_APP_API_URL + '/create-Business';
    const requestData = {
      Name: formData.Name,
      UserName: formData.UserName,
      Password: formData.Password,
      Address: {
        Adress: formData.Address.Adress,
        PostCode: formData.Address.PostCode,
      },
      URL: formData.URL
    };

    axios
      .post(backendEndpoint, requestData)
      .then((response) => {
      })
      .catch((error) => {
        console.error('Error submitting form to backend:', error);
      });
  };

  return (
    <div>
      <h1>Registreer</h1>
      <Form className="custom-form-control">
        <>
          {!formData.isValidConstraint && (
            <Form.Text className="text-danger">{formData.errorMessage}</Form.Text>
          )}
          <Form.Group className="mb-3 " controlId="formBasicPostalCode">
            <Form.Label>Vul je Postcode in</Form.Label>
            <Form.Control type="PostCode" required value={formData.Address.PostCode} onChange={(e) => handleInputChange('Address.PostCode', e.target.value)} placeholder="vul in Postcode"/>
          </Form.Group>
          <Form.Group className="mb-3 " controlId="formBasicHome">
            <Form.Label>Vul je Adress in</Form.Label>
            <Form.Control type="Adress" value={formData.Adress} onChange={(e) => handleInputChange('Adress', e.target.value)} placeholder="vul in Adress"/>
            <Form.Label>Vul je website url in</Form.Label>
            <Form.Control type="URL" value={formData.URL} onChange={(e) => handleInputChange('URL', e.target.value)} placeholder="vul in URL "/>
          </Form.Group>
          <Form.Group className="mb-3" controlId="formBasicUserName">
            <Form.Label>Vul je bedrijfsnaam in</Form.Label>
            <Form.Control type="Name" value={formData.Name} onChange={(e) => handleInputChange('Name', e.target.value)} placeholder="vul in bedrijfsnaam"/>
          </Form.Group>
          <Form.Group className="mb-3" controlId="formBasicUserName">
            <Form.Label>Vul je gebruikersnaam in</Form.Label>
            <Form.Control type="UserName" value={formData.UserName} onChange={(e) => handleInputChange('UserName', e.target.value)} placeholder="vul in gebruikersnaam"/>
          </Form.Group>
          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Vul je wachtwoord in</Form.Label>
            <Form.Control type="Password" value={formData.Password} onChange={(e) => handleInputChange('Password', e.target.value)} placeholder="vul in wachtwoord"/>
          </Form.Group>
          <Button variant="primary" type="button" href='/' onClick={submit}> Registreer </Button>
        </>
      </Form>
    </div>
  );
};

export default Register;
