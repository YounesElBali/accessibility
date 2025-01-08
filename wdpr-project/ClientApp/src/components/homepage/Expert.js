import React, { Component } from 'react';
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';


export class ExpertHome extends Component {
    static displayName = ExpertHome.name;

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
              <Card.Title>Onderzoek</Card.Title>
              <Button variant="primary" href="/onderzoeken">meld aan voor onderzoek</Button>
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