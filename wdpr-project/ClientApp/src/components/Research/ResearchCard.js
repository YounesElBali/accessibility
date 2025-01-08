import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col'
import Button from 'react-bootstrap/Button'
import './Research.css'

function ResearchCard(props) {
    return (
        <Col xs={12} lg={6} xxl={4}>
            <Card className="Research-card" key={props.researchId} >
                <Card.Body>
                    <Card.Title><h2>{props.researchTitle}</h2></Card.Title>
                    <Card.Text>{props.researchDescription}</Card.Text>
                </Card.Body>
                <Button variant="primary" href={"Onderzoeken/" + props.researchId}>lees meer over {props.researchTitle}</Button>
            </Card>
        </Col>
    )
}

export default ResearchCard;