import {NavItem, NavLink} from "reactstrap";
import { Link } from 'react-router-dom';
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';

const uitloggen = () => {
    localStorage.removeItem("token");
    this.setState({
        toegang: localStorage.getItem("toegang"),
    });
    window.location.href = '/';
}

const AdminNavMenu = () => {
    return (<>
        <DropdownButton id="dropdown-basic-button" title="Lijsten" variant="success">
            <Dropdown.Item href="/expert-list">Lijst van onderzoeken</Dropdown.Item>
            <Dropdown.Item href="/research-list">Lijst van ervaringsdeskundige</Dropdown.Item>
            <Dropdown.Item href="/business-list">Lijst van bedrijven</Dropdown.Item>
        </DropdownButton>
        <NavItem>
            <NavLink tag={Link} className="text-light" to="/admin">
                Gegevens
            </NavLink>
        </NavItem>
        <NavItem>
            <NavLink tag={Link} className="text-light" to="/chatIndex">
                Chat
            </NavLink>
        </NavItem>
        <NavItem>
            <NavLink tag={Link} id='signOut' className="text-light"  to="/" onClick={uitloggen}>
                Logout
            </NavLink>
        </NavItem> 
    </>)
}

export default AdminNavMenu;