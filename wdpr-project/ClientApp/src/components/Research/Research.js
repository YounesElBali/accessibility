import React, { useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container'
import Row from 'react-bootstrap/Row'
import ResearchCard from "./ResearchCard";

const Research = () => {
    
    const [researches, setResearches] = useState([]);
    useEffect(() => {
        fetch(process.env.REACT_APP_API_URL + '/Research')
            .then((response) => response.json())
            .then((data) => {
                 setResearches(data);
            })
            .catch((err) => {
                console.log(err.message);
            });
    }, []);
    
        return (
            <Container className="Research-container">
                <h1>Onderzoeken</h1>
                <Row>
                    {researches.map((research => {
                        return (
                            <ResearchCard key={research.id} researchId={research.id} researchTitle={research.title} researchDescription={research.description}></ResearchCard>
                        )
                    }))}
                </Row>
            </Container>
        )
}

export default Research;