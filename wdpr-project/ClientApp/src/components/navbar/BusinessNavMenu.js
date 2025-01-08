import {NavItem, NavLink} from "reactstrap";
import { Link } from 'react-router-dom';
import { useState } from 'react';
import { Home } from "../Home";
const BusinessNavMenu = () => {
    // eslint-disable-next-line
    const [toegang, setToegang] = useState(localStorage.getItem("toegang"));

    const uitloggen = () => {
      
        localStorage.removeItem("token");
        setToegang(localStorage.getItem("toegang"));
        <Home/>
        window.location.href = '/';
    
    }
    return (<>
        <NavItem>
            <NavLink tag={Link} className="text-light" to="/research-overview">
                Onderzoeken
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

export default BusinessNavMenu;