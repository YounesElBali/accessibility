import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import { Button } from 'react-bootstrap';

const ResearchOverview = () => {
  const [researchData, setResearchData] = useState([]);

  useEffect(() => {
    const fetchResearchData = async () => {
        const currentUserId = getCurrentUser();
      try {
        const response = await axios.post(process.env.REACT_APP_API_URL +'/researchess', {
          CurrentUserId: currentUserId, 
        });
        setResearchData(response.data);
      } catch (error) {
        console.error('Error fetching research data:', error);
      }
    };
    fetchResearchData();
  }, []);

  const getCurrentUser = () => {
    let token = localStorage.getItem('token');
    const decoded = jwtDecode(token);
    const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    console.log("business: " + userId);
    return userId;
  };

  return (
    <div>
      <h1>Overzicht van mijn onderzoeken en de deelnemers</h1>

      <Button variant="success" href='/create-research'>
        Maak een nieuw onderzoek aan
      </Button>
      <Row>
      {researchData.map((research) => (
        <Col key={research.id} xs={12} lg={6} xxl={4}>
        <Card className="Card-card" key={research.id} >
            <Card.Body>
                <Card.Title><h2>Titel: {research.title}</h2></Card.Title>
                <br />
                <Card.Title> Omschrijving: {research.description}</Card.Title>
                <Card.Title>Beloning: {research.reward}</Card.Title>
                <Card.Title> Capaciteit: {research.capacity}</Card.Title>
                <Card.Title> Status onderzoek: {research.status ? 'actief' : 'in actief'}</Card.Title>
                <br />
                <Card.Title>Ervaringsdeskundige gebruikersnaam: {research.expertIds.join(', ')}</Card.Title>
            </Card.Body>
            </Card>
        </Col>
        ))}
     </Row>
    </div>

  );
};

export default ResearchOverview;
