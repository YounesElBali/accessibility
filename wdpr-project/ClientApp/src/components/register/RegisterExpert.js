/* eslint-disable no-undef */
import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import '../register/register.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';


const RegisterExpert = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const history = useNavigate();
  const [formData, setFormData] = useState({
    PersonalData: {
      Firstname: '',
      Middlenames: '',
      Lastname: '',
      Emailaddress: '',
      Phonenumber: '',
      Age: null,
      Address: {
        Adress: '',
        Postcode: '',
      },
    },
    ContactByPhone: false,
    ContactByThirdParty: false,
    Disabilities: [
      {
        Type: 'test',
        Description: 'test',
      },
    ],
    Aids: [
      {
        Description: 'test',
      },
    ],
    UserName: '',
    Password: '',
    ResearchExperts: null
  });
  const handleInputChange = (field, value) => {

    if (field === 'PersonalData.Age') {
      // Ensure value is a valid number between 0 and 100, or set it to null
      value = value === '' ? null : Math.max(0, Math.min(100, parseInt(value, 10)));
    }
  
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

  const nextPage = () => {
    setCurrentPage((prevPage) => prevPage + 1);
  };

  const previousPage = () => {
    setCurrentPage((prevPage) => prevPage - 1);
  };

  const submit = () => {
  
    const backendEndpoint = process.env.REACT_APP_API_URL +'/create';
    
    axios
      .post(backendEndpoint, formData)
      .then((response) => {
        history('/');
        window.location.reload();
      })
      .catch((error) => {
        console.error('Error submitting form to backend:', error);
      });
  };

        return(
            <div>
                <h1>Registreer</h1>
            <Form className="custom-form-control">
            {currentPage === 1 && (
                <>
                    <Form.Group className="mb-3" controlId="formBasicName">
                        <Form.Label>Vul je voornaam in</Form.Label>
                        <Form.Control type="Firstname" data-testid="firstname" value={formData.PersonalData.Firstname} onChange={(e) => handleInputChange('PersonalData.Firstname', e.target.value)} placeholder="vul in voornaam"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicMiddleName">
                        <Form.Label>Vul tussenvoegsel in</Form.Label>
                        <Form.Control type="Middlenames"  data-testid="middlenames" value={formData.PersonalData.Middlenames} onChange={(e) => handleInputChange('PersonalData.Middlenames', e.target.value)} placeholder="vul in tussenvoegsel"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicLastName">
                        <Form.Label>Vul je achternaam in</Form.Label>
                        <Form.Control type="LastName" data-testid="lastname" value={formData.PersonalData.LastName} onChange={(e) => handleInputChange('PersonalData.LastName', e.target.value)} placeholder="vul in achternaam"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicAge">
                        <Form.Label>Vul je leeftijd in</Form.Label>
                        <Form.Control type="number" min="0" max="100" data-testid="age" value={formData.PersonalData.Age} onChange={(e) => handleInputChange('PersonalData.Age', e.target.value)} placeholder="vul in leeftijd"/>
                    </Form.Group>
                    <Form.Group className="mb-3 " controlId="formBasicPhoneNumber">
                        <Form.Label>Vul je telefoonnummer in </Form.Label>
                        <Form.Control type="Phonenumber" data-testid="phonenumber" value={formData.PersonalData.Phonenumber} onChange={(e) => handleInputChange('PersonalData.Phonenumber', e.target.value)} placeholder="vul in telefoonnummer" />
                    </Form.Group>
                    <Form.Group className="mb-3 " controlId="formBasicEmail">
                        <Form.Label>Vul je email adres in </Form.Label>
                        <Form.Control type="Emailaddress" data-testid="emailaddress"  value={formData.PersonalData.Emailaddress} onChange={(e) => handleInputChange('PersonalData.Emailaddress', e.target.value)} placeholder="vul in email" />
                    </Form.Group>
                    <Form.Group className="mb-3 " controlId="formBasicPostalCode">
                        <Form.Label>Vul je Postcode in</Form.Label>
                        <Form.Control type="PostCode" data-testid="postcode" value={formData.PersonalData.Address.PostCode} onChange={(e) => handleInputChange('PersonalData.Address.PostCode', e.target.value)} placeholder="vul in postcode" />
                        <Form.Label>Vul je Adress in</Form.Label>
                        <Form.Control type="Adress" data-testid="adress" value={formData.PersonalData.Address.Adress} onChange={(e) => handleInputChange('PersonalData.Address.Adress', e.target.value)}  placeholder="vul in adress" />
                    </Form.Group>
                    <Button variant="primary" type="button" data-testid="next-button" onClick={nextPage}> Volgende </Button>
                </>
            )}
            {currentPage === 2 && (
                <>
                    <Form.Group className="mb-3" controlId="formBasicUserName">
                        <Form.Label>Vul je gebruikersnaam in</Form.Label>
                        <Form.Control type="UserName" data-testid="username" value={formData.UserName} onChange={(e) => handleInputChange('UserName', e.target.value)} placeholder="vul in gebruikersnaam" />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicPassword">
                        <Form.Label>Vul je wachtwoord in</Form.Label>
                        <Form.Control type="Password" data-testid="password" value={formData.Password} onChange={(e) => handleInputChange('Password', e.target.value)} placeholder="vul in wachtwoord" />
                    </Form.Group>
                    <Button variant="primary" type="button"  onClick={previousPage }> Vorige </Button>
                    <Button variant="primary" type="button" data-testid="submit-button" href='/' onClick={submit}> Registreer </Button>
                </>
             )}  
            </Form> 
            </div>
        );
    };

export default RegisterExpert;