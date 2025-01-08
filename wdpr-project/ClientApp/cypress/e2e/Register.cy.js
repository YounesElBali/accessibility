/// <reference types="cypress" />

describe('Expert Registration Form', () => {
    it('Successfully submits expert registration form', () => {
      cy.visit('https://localhost:44428/register-consultant'); // Replace with the actual port of your application
  
      // Fill in the registration form
      cy.get('[data-testid="firstname"]').type('John');
      cy.get('[data-testid="middlenames"]').type('Doe');
      cy.get('[data-testid="lastname"]').type('Smith');
      cy.get('[data-testid="age"]').type('25');
      cy.get('[data-testid="phonenumber"]').type('123456789');
      cy.get('[data-testid="emailaddress"]').type('john.doe@example.com');
      cy.get('[data-testid="postcode"]').type('12345');
      cy.get('[data-testid="adress"]').type('123 Main St');

      cy.get('[data-testid="next-button"]').click();

      cy.get('[data-testid="username"]').type('john_doe');
      cy.get('[data-testid="password"]').type('Password123@');

      cy.get('[data-testid="submit-button"]').click();
      cy.url().should('include', '/');
    });
  });
  