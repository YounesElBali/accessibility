import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col'
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import Row from 'react-bootstrap/Row'
import './list.css';

const ResearchList = () => {
  const [researches, setResearches] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('title');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(process.env.REACT_APP_API_URL +'/admin/get/research');
        const data = response.data;
        setResearches(data);
      } catch (error) {
        console.error('Error fetching researches:', error.message);
      }
    };

    fetchData(); // Call fetchData when the component mounts
  }, []); // Empty dependency array ensures the effect runs only once

  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  const handleSortChange = (selectedKey) => {
    setSortBy(selectedKey);
  };

  // Filter and sort the researches based on search term and sort criteria
  const filteredAndSortedResearches = researches
    .filter((research) =>
    research.title.toLowerCase().includes(searchTerm.toLowerCase())
    )
    .sort((a, b) => {
        if (sortBy === 'A-Z') {
          return a.title.localeCompare(b.title);
        } else if (sortBy === 'Z-A') {
          return a.title.localeCompare(b.title) * -1; // Reverse the order
        }
        return 0;
    });

    return (
        <div>
          <h1>Onderzoek lijst</h1>
          <br />
          <Col xs={12} lg={6} xxl={4}>
            <InputGroup className="mb-6">
              <Form.Control aria-label="Zoekveld met lijst van sorteer opties" placeholder="Zoek bij onderzoeken" value={searchTerm} onChange={handleSearchChange} />
              <DropdownButton variant="success" title="Sorteer" id="input-group-dropdown-2" align="end" onSelect={handleSortChange}>
                <Dropdown.Item eventKey="title">Titel</Dropdown.Item>
                <Dropdown.Item eventKey="A-Z">A-Z</Dropdown.Item>
                <Dropdown.Item eventKey="Z-A">Z-A</Dropdown.Item>
              </DropdownButton>
            </InputGroup>
          </Col>
          <br />
          <br />
          <Row>
            {filteredAndSortedResearches.map((research) => (
              <Col key={research.id} xs={12} lg={6} xxl={4}>
                <Card className="Card-card">
                  <Card.Body>
                    <Card.Title><h2>Titel: {research.title} </h2></Card.Title>
                    <br />
                    <Card.Text>Omschrijving: {research.description}</Card.Text>
                    <Card.Text>Beloning: {research.reward}</Card.Text>
                    <Card.Text>Capaciteit: {research.capacity}</Card.Text>
                    <Card.Text>Status: {research.status ? 'Actief' : 'Inactief'}</Card.Text>
                    {research.researchCriterium ? (
                      <>
                        <Card.Text>Minimum leeftijd: {research.researchCriterium.minmumAge}</Card.Text>
                        <Card.Text>Maximum leeftijd: {research.researchCriterium.maximumAge} </Card.Text>
                        <Card.Text>Adres: {research.researchCriterium.address} </Card.Text>
                        <Card.Text>Beperkingstype: {research.researchCriterium.disability.type} </Card.Text>
                        <Card.Text>Beperking Omschrijving: {research.researchCriterium.disability.description} </Card.Text>
                      </>
                    ) : (<strong>geen onderzoek criteria</strong>)}
                  </Card.Body>
                </Card>
              </Col>
            ))}
          </Row>
        </div>
        
      );
      
};

export default ResearchList;
