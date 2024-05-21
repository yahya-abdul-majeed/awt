
export default function WebsiteTree({websites}){
    console.log("from website")
    return(
        <div id="website-tree-main">
            {
                websites.map((website)=>{
                    return <Node website={website}/>
                })
            }
        </div>
    );
}


function Node({website}){
    const isLeaf = website.children.length > 0 ? false : true;

    if(isLeaf){
        return(
            <div className="website-item-parent">
                <div className="website-item">
                    <input id={`assignment-website-${website.id}`} type="checkbox" name="assignment-website-checkbox" value={website.id}/>
                    {website.hosted_at}<br/>
                    <input id={`pass-${website.id}`} type="radio" name={`should_pass_${website.id}`} defaultChecked/>
                    <label for={`pass-${website.id}`}>pass</label>
                    <input id={`fail-${website.id}`} type="radio" name={`should_pass_${website.id}`}/>
                    <label for={`fail-${website.id}`}>fail</label>
                </div>
            </div>
        );
    }else{
        return(
            <div className="website-item-parent">
                <div className="website-item">
                    <input id={`assignment-website-${website.id}`} type="checkbox" name="assignment-website-checkbox" value={website.id}/>
                    {website.hosted_at}<br/>
                    <input id={`pass-${website.id}`} type="radio" name={`should_pass_${website.id}`} defaultChecked/>
                    <label for={`pass-${website.id}`}>pass</label>
                    <input id={`fail-${website.id}`} type="radio" name={`should_pass_${website.id}`}/>
                    <label for={`fail-${website.id}`}>fail</label>
                </div>
                <div style={{
                    marginLeft:40
                }}>
                    {
                        website.children.map((website)=>{
                            return <Node website={website}/>
                        })
                    }
                </div>
            </div>
        );

    }
}