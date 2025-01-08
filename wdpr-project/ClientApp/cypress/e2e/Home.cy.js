// cypress/integration/homepage.spec.js

describe('Login Form', () => {
  beforeEach(() => {
    cy.visit('https://localhost:44428/login'); 
  });

  it('should successfully log in with valid credentials', () => {
    cy.get('#formBasicUserName').type('john_doe');
    cy.get('#formBasicPassword').type('Password123@');
    cy.get('button[type="submit"]').click();
    cy.get('#signOut').click();
    cy.url().should('include', '/expertHome');
  });

  it('should display an error message for invalid credentials', () => {
    cy.get('#formBasicUserName').type('Ervaringsdekundige1');
    cy.get('#formBasicPassword').type('Ervaringsdekundige1#');
    cy.get('button[type="submit"]').click();

    cy.get('.alert-danger').should('be.visible');
  });


});

  