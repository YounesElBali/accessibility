import React, { Component } from "react"

export class Footer extends Component {
    static displayName = Footer.name;
   
    constructor (props) {
        super(props);

        this.footer = this.footer.bind(this);
        this.state = {
        collapsed: true
        };
    }

    footer () {
    this.setState({
      collapsed: !this.state.collapsed
    });
    }

    render() {
        return (
            <footer className="page-footer font-small blue pt-1 text-center" style={{ backgroundColor: '#2B50EC', color: 'white' }}>
            <div className="container-fluid text-md-left">
              <div className="row">
                  <h5 className="text-uppercase text-center">Contact</h5>
                  <p>030 - 239 82 70</p>
                  <p>info@accessibility.nl</p>
              </div>
            </div>
            <div className="py-3 pt-1">
              © 2023 Stichting Accessibility
              <a href="https://www.youtube.com/channel/UCSFsnRBNIDCgYJEW_ZLfTrg" target="_blank" rel="noopener noreferrer" className="text-light mx-2">Facebook</a>
              <a href="https://twitter.com/AccessibilityNL" target="_blank" rel="noopener noreferrer" className="text-light mx-2">Twitter</a>
              <a href="https://nl.linkedin.com/company/accessibilitynl" target="_blank" rel="noopener noreferrer" className="text-light mx-2">Linkedin</a>
            </div>
          </footer>
        );
    };
}