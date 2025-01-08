import React, { Component } from 'react';
import { Carousel } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';


export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div className="text-center" data-interval="2"> 
        <Carousel>
          <Carousel.Item>
            <img
            class="responsive"
              src="Images/img1.jpg"
              alt="Male patient with thumbs up next to a female doctor"
            />
          </Carousel.Item>
          <Carousel.Item>
            <img
              class="responsive"
              src="Images/img2.jpg"
              alt="two pairs of hands holding eachother"
            />
          </Carousel.Item>
          <Carousel.Item>
            <img
            class="responsive"
              src="Images/img3.jpg"
              alt="A female nurse helping an elderly man using a walking device"
            />
          </Carousel.Item>
          <Carousel.Item>
            <img
            class="responsive"
              src="Images/img4.jpg"
              alt="A female nurse and elderly man in a garden"
            />
          </Carousel.Item>
          <Carousel.Item>
            <img
            class="responsive"
              src="Images/img5.jpg"
              alt="A male nurse showing a paper to an elderly woman"
            />
          </Carousel.Item>
        </Carousel>

        <div background-color='2B50EC'>
        <h1 class="block">
      
          Stichting Accessibility
        </h1>
        </div>
        <p class="wrapper">  
          Welkom op de Stichting Accessibility Webapplicatie – waar gelijkwaardige
          participatie centraal staat! Onze missie is het creëren van een inclusieve samenleving,
          en deze webapplicatie dient als een brug tussen mensen met diverse beperkingen,
          bedrijven en onderzoekers. Hier kunnen individuen met een beperking zich registreren
          als ervaringsdeskundigen, bedrijven vinden die graag hun online toegankelijkheid 
          willen verbeteren, en onderzoekers cruciale inzichten verkrijgen. 
          Samen streven we naar digitale, fysieke en sociale toegankelijkheid voor iedereen. 
          Ontdek de kracht van inclusiviteit en maak deel uit van onze gemeenschap!
        </p>
      </div>
    );
  }
}