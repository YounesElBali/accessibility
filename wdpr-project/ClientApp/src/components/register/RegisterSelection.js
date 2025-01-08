import React, { Component } from 'react';
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';


export class RegisterSelection extends Component {
    static displayName = RegisterSelection.name;

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
              <Card.Title>Registreer als bedrijf</Card.Title>
              <Button variant="primary" href="/register-business">Registreer als Bedrijf</Button>
            </Card.Body>
          </Card>
        </Col>

        <Col md={3}>
          <Card style={{ width: '18rem' }}>
            <Card.Body>
              <Card.Title>Registreer als Ervaringsdeskundige</Card.Title>
              <Button variant="primary" href="/register-consultant">Registreer als Ervaringsdeskundige</Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>
            
        );
    }
}