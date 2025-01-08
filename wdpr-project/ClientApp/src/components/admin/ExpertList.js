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

const ExpertList = () => {
  const [experts, setExperts] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('userName');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(process.env.REACT_APP_API_URL +'/admin/get/expert');
        const data = response.data;
        setExperts(data);
      } catch (error) {
        console.error('Error fetching experts:', error.message);
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

  // Filter and sort the experts based on search term and sort criteria
  const filteredAndSortedExperts = experts
    .filter((expert) =>
      expert.userName.toLowerCase().includes(searchTerm.toLowerCase())
    )
    .sort((a, b) => {
      if (sortBy === 'A-Z') {
        return a.userName.localeCompare(b.userName);
      } else if (sortBy === 'Z-A') {
        return b.userName.localeCompare(a.userName) * -1;
      }
      return 0;
    });

  return (
    <div>
      <h1>Lijst van ervaringsdeskundige</h1>
        <br/>
        <Col xs={12} lg={6} xxl={4}>
            <InputGroup className="mb-6">
            <Form.Control aria-label="Zoekveld met lijst van sorteer opties" placeholder="Zoek bij ervaringsdeskundige" value={searchTerm} onChange={handleSearchChange}/>
                <DropdownButton variant="success" title="Sorteer" id="input-group-dropdown-2" align="end" onSelect={handleSortChange}> 
                <Dropdown.Item  eventKey="userName">Naam</Dropdown.Item>
                <Dropdown.Item  eventKey="A-Z">A-Z</Dropdown.Item>
                <Dropdown.Item  eventKey="Z-A">Z-A</Dropdown.Item>
                </DropdownButton>
            </InputGroup>
        </Col>
      <br/>
      <br/>
      <Row>
        {filteredAndSortedExperts.map((expert) => (
            <Col xs={12} lg={6} xxl={4} key={expert.id}>
                    <Card className="Card-card" key={expert.id} >
                        <Card.Body>
                            <Card.Title><h2>Naam: {expert.personalData.firstname} {expert.personalData.middlenames} {expert.personalData.lastname} </h2></Card.Title>
                            <br/>
                            <Card.Text>Email: {expert.personalData.emailaddress}</Card.Text>
                            <Card.Text>Telefonisch bereikbaar: {expert.contactByPhone ? 'Ja' : 'Nee'}</Card.Text>
                            <Card.Text>Contact door derden: {expert.contactByThirdParty ? 'Ja' : 'Nee'}</Card.Text>
                            <Card.Text>Beperkingen: {expert.disabilities.map((disability) => disability.description).join(', ')}</Card.Text>
                            <Card.Text>Hulpmiddelen: {expert.aids.map((aid) => aid.description).join(', ')}</Card.Text>
                            <Card.Text>Telefoonnummer: {expert.personalData.phonenumber}</Card.Text>
                            <Card.Text>Leeftijd: {expert.personalData.age}</Card.Text>
                            <Card.Text>Adres + postcode: {expert.personalData.address.adress}, {expert.personalData.address.postcode}</Card.Text>
                            <Card.Text>Aangemelden onderzoeken: {expert.personalData.research}</Card.Text>
                        </Card.Body>
                    </Card>
                </Col>
        ))}    
      </Row>
    </div>
  );
};

export default ExpertList;
