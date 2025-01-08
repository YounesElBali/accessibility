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
import Access from '../login/Access';
const BusinessList = () => {
  const [businesses, setBusinesses] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('url');

  useEffect(() => {
    const fetchData = async () => {
   let token = await Access();
      try {
        const response = await axios.get(process.env.REACT_APP_API_URL +'/admin/get/business'
          ,{headers: {
              'Content-type': 'application/json',
              'Authorization': `Bearer ${token}`, // notice the Bearer before your token
          }
        });
        const data = response.data;
        setBusinesses(data);
      } catch (error) {
        console.error('Error fetching businesses:', error.message);
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

  // Filter and sort the businesses based on search term and sort criteria
  const filteredAndSortedBusinesses = businesses
    .filter((business) =>
      business.name.toLowerCase().includes(searchTerm.toLowerCase())
    )
    .sort((a, b) => {
        if (sortBy === 'A-Z') {
          return a.name.localeCompare(b.name);
        } else if (sortBy === 'Z-A') {
          return b.name.localeCompare(a.name) * -1;
        }
        return 0;
    });

  return (
    <div>
      <h1>Lijst met bedrijven</h1>

      <Col xs={12} lg={6} xxl={4}>
            <InputGroup className="mb-6">
            <Form.Control aria-label="Zoekveld met lijst van sorteer opties" placeholder="Zoek naar bedrijven" value={searchTerm} onChange={handleSearchChange}/>
                <DropdownButton variant="success" title="Sorteer" id="input-group-dropdown-2" align="end" onSelect={handleSortChange}> 
                <Dropdown.Item  eventKey="name">Bedrijfsnaam</Dropdown.Item>
                <Dropdown.Item  eventKey="A-Z">A-Z</Dropdown.Item>
                <Dropdown.Item  eventKey="Z-A">Z-A</Dropdown.Item>
                </DropdownButton>
            </InputGroup>
        </Col>
        <br/>
        <br/>
      <Row>
        {filteredAndSortedBusinesses.map((business) => (
            <Col key={business.id} xs={12} lg={6} xxl={4}>
                    <Card className="Card-card" key={business.id} >
                        <Card.Body>
                            <Card.Title><h2>Bedrijfsaam: {business.name} </h2></Card.Title>
                            <br/>
                            <Card.Text>URL: {business.url}</Card.Text>
                            <Card.Text>Bedrijfsaam: {business.name}</Card.Text>
                            <Card.Text>Bedrijfsonderoeken: </Card.Text>
                        </Card.Body>
                    </Card>
            </Col>
        ))}
      </Row>
    </div>
  );
};

export default BusinessList;
