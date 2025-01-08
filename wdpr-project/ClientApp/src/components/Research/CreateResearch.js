import React, { useState, useEffect } from 'react';
import axios from 'axios';
import ListGroup from 'react-bootstrap/ListGroup';
import { Button, Form } from 'react-bootstrap';
import { jwtDecode } from 'jwt-decode';

const CreateResearch = () => {
  const [researchData, setResearchData] = useState({
    Title: '',
    Description: '',
    Reward: '',
    Capacity: '',
    Status: true,
    businessId: 0,
    ResearchCriterium: {
      MinimumAge: '',
      MaximumAge: '',
      Address: '',
    }
  });
  const [disabilities, setDisabilities] = useState([]);
  const [disability, setDisability] = useState({
    DisabilityId : 0
});
  useEffect(() => {
    // Fetch existing disabilities when the component mounts
    const authToken = localStorage.getItem('token');
    const fetchDisabilities = async () => {
      try {
        const response = await axios.get(process.env.REACT_APP_API_URL + '/Disability'
        ,{headers: {
            'Content-type': 'application/json',
            'Authorization': `Bearer ${authToken}`, // notice the Bearer before your token
        }
      });
        setDisabilities(response.data);
      } catch (error) {
        console.error('Error fetching disabilities:', error);
        // Handle errors (display a message, log, etc.)
      }
    };

    fetchDisabilities();
  }, []); // The empty dependency array ensures that the effect runs only once

 const handleDisabilityClick = (selectedDisabilityId) => {
    console.log(selectedDisabilityId);
    
    setDisability((prevData) => ({
      ...disability,
      DisabilityId: selectedDisabilityId,
    }));
  };
  
  
  

  const handleSubmit = async (e) => {
    e.preventDefault();
    const currentUserId = getCurrentUser();
    const updatedResearchData = {
      ...researchData,
      businessId: currentUserId,
    };

    try {
    const response = await axios.post(process.env.REACT_APP_API_URL +'/Create-Research', updatedResearchData);
      console.log('Research created:', response.data);
      // Optionally, you can redirect the user or perform other actions after successful creation
    } catch (error) {
      console.error('Error creating research:', error);
      // Handle errors (display a message, log, etc.)
    }
  };

  const getCurrentUser = () => {
    let token = localStorage.getItem('token');
    const decoded = jwtDecode(token);
    const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    console.log("business: " + userId);
    return userId;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
  
    if (name === 'MaximumAge' || name === 'MinimumAge') {
      // Ensure value is a valid number between 0 and 100, or set it to null
      const newValue = value === '' ? null : Math.max(0, Math.min(100, parseInt(value, 10)));
      setResearchData((prevData) => ({
        ...prevData,
        ResearchCriterium: {
          ...prevData.ResearchCriterium,
          [name]: newValue,
        },
      }));
    } else if (name === 'Address') {
      setResearchData((prevData) => ({
        ...prevData,
        ResearchCriterium: {
          ...prevData.ResearchCriterium,
          [name]: value,
        },
      }));
    } else {
      setResearchData((prevData) => ({
        ...prevData,
        [name]: value,
      }));
    }
  };  

  return (
    <Form onSubmit={handleSubmit}>
      <Form.Group className="mb-3" controlId="formResearchTitle">
        <Form.Label>Titel:</Form.Label>
        <Form.Control type="text" name="Title" value={researchData.Title} onChange={handleChange} placeholder="vul in titel" />
      </Form.Group>
      <Form.Group className="mb-3" controlId="formResearchDescription">
        <Form.Label>Omschrijving:</Form.Label>
        <Form.Control as="textarea" name="Description" value={researchData.Description} onChange={handleChange} placeholder="vul in omschrijving" />
      </Form.Group>
      <Form.Group className="mb-3" controlId="formResearchReward">
        <Form.Label>Beloning:</Form.Label>
        <Form.Control type="number" name="Reward" value={researchData.Reward} onChange={handleChange} placeholder="vul in beloning" />
      </Form.Group>
      <Form.Group className="mb-3" controlId="formResearchCapacity">
        <Form.Label>Capaciteit:</Form.Label>
        <Form.Control type="number" name="Capacity" value={researchData.Capacity} onChange={handleChange} placeholder="vul in capaciteit" />
      </Form.Group>
      <Form.Group className="mb-3" controlId="formResearchMaximumAge">
        <Form.Label>Maximum leeftijd:</Form.Label>
        <Form.Control type="number" name="MaximumAge" value={researchData.ResearchCriterium.MaximumAge} onChange={handleChange} placeholder="vul in Maximum leeftijd" />
      </Form.Group>
      <Form.Group className="mb-3" controlId="formResearchMinimumAge">
        <Form.Label>Minimum leeftijd:</Form.Label>
        <Form.Control type="number" name="MinimumAge" value={researchData.ResearchCriterium.MinimumAge} onChange={handleChange} placeholder="vul in Minimum leeftijd" />
      </Form.Group>
      <Form.Group className="mb-3" controlId="formResearchAddress">
        <Form.Label>Adres:</Form.Label>
        <Form.Control type="text" name="Address" value={researchData.ResearchCriterium.Address} onChange={handleChange} placeholder="vul in adres" />
      </Form.Group>
      <ListGroup>
  {disabilities.map((disability) => (
    <div key={disability.id}>
      <ListGroup.Item
        action
        onClick={() => handleDisabilityClick(disability.id)}
        active={disability.DisabilityId === disability.id}
      >
        {disability.type}
      </ListGroup.Item>
      <br />
    </div>
  ))}
</ListGroup>

           
                <br/>

      <Button variant="primary" type="submit">
        Maak onderzoek
      </Button>
    </Form>
  );
};


export default CreateResearch;
