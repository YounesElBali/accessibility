import React, { Component } from 'react';
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';


export class AdminHome extends Component {
    static displayName = AdminHome.name;

    constructor (props) {
        super(props);
    
        
        this.state = {
          collapsed: true
        };
    }

    render(){
        return(
            <Row>
        <Col md={4}>
          <Card style={{ width: '18rem', marginBottom: '20px' }}>
            <Card.Body>
              <Card.Title>Overzicht gebruikers</Card.Title>
              <Button variant="primary" href="/onderzoeken">bekijk gegevens onderzoeken</Button>
            </Card.Body>
          </Card>
        </Col>

        <Col md={3}>
          <Card style={{ width: '18rem' }}>
            <Card.Body>
              <Card.Title>Chat</Card.Title>
              <Button variant="primary" href="/chatIndex">stuur bericht</Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>
            
        );
    }
}