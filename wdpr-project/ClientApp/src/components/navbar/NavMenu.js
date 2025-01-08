import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import Image from 'react-bootstrap/Image';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import LoggedoutNavMenu from "./LoggedoutNavMenu";
import {jwtDecode} from "jwt-decode";
import ExpertNavMenu from "./ExpertNavMenu";
import AdminNavMenu from "./AdminNavMenu";
import BusinessNavMenu from "./BusinessNavMenu";

export class NavMenu extends Component {
  static displayName = NavMenu.name;
  
  constructor(props) {
    
    super(props);
    this.uitloggen = this.uitloggen.bind(this);
    this.redirect = this.redirect.bind(this);
    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      toegang: localStorage.getItem("toegang"),
      collapsed: true,
      isLoggedIn: false,
      userRole: null
    };
  }

  componentDidMount() {
    // Check if the user is logged in when the component mounts
    this.checkLoginStatus();
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed,
    });
  }

  checkLoginStatus() {
    try {
      let token = localStorage.getItem('token');  
      const isLoggedIn = token != null;
      const userRole = jwtDecode(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      
      this.setState({
        isLoggedIn,
        userRole
      });
    } catch {
      this.setState({
        isLoggedIn: false,
        userRole: null
      });
    }
  }
  uitloggen() {
    localStorage.removeItem("token");
    this.setState({
      toegang: localStorage.getItem("toegang"),
    });
    window.location.href = '/';
  }
  
  renderSwitch() {
    var userRole = this.state.userRole;
    switch(userRole){
      case "Expert":
        return <ExpertNavMenu/>
      case "Admin":
        return <AdminNavMenu/>
      case "Business":
        return <BusinessNavMenu/>
      default:
        return <>
          <NavItem>
            <NavLink tag={Link} id='signOut' className="text-light"  to="/" onClick={this.uitloggen}>
              Logout
            </NavLink>
          </NavItem>
        </>
    }
  }

   redirect()  {
    const userRole = localStorage.getItem("role");
    if (userRole) {
      switch (userRole) {
        case 'Expert':
          this.props.history.push('/expertHome');
          break;
        case 'Business':
          this.props.history.push('/businessHome');
          break;
        case 'Admin':
          this.props.history.push('/adminHome');
          break;
        default:
          this.props.history.push('/');
      }
  }}
  render() {
     const { isLoggedIn } = this.state;

     return (
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light style={{ backgroundColor: '#2B50EC' }}>
          <NavbarBrand tag={Link} to="/">
            <Image src={require('../navbar/logo.png')} className="logo" alt="Logo of Stichting Accessibility" />
          </NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              {isLoggedIn ? (this.renderSwitch()) : (
                <LoggedoutNavMenu/>
                )}
            </ul>
          </Collapse>
        </Navbar>
    );
  }
}
