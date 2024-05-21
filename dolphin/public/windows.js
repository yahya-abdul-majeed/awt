
//for some reason, closeHandler has to be defined outside document.ready handler for it to work.

/* Event handlers */
function closeHandler(event){
    $(this).window('destroy')
}


/* Main jQuery code */
$(function(){

window.createContestWindow = function createContestWindow(event){
    let win = $('#winContest');
    if( win.css('display') != 'none') {
        win.window('open')
    }else{
        win.window({
            width:1000,
            height:600,
            modal:true
        });
        win.show();
    }


}

window.createAssignmentWindow = function createAssignmentWindow(event){
    let win = $('#winAssignment');
    if( win.css('display') != 'none') {
        win.window('open')
    }else{
        win.window({
            width:1000,
            height:600,
            modal:true
        });
        win.show();
    }

}

window.createGroupWindow = function createGroupWindow(event){
    let win = $('#winGroup');
    if( win.css('display') != 'none') {
        win.window('open')
    }else{
        win.window({
            width:1000,
            height:600,
            modal:true
        });
        win.show();
    }
}
let left = 500;
let top = 200;

window.createNewWindowEachTime = function createNewWindowEachTime(){
    let id = Math.floor(Math.random() * 1000) + 1;

    
    let element = `<div id='${id}' title='Contests' border='thin' data-options='onClose:closeHandler' left:'100' top:'100' minimizable='false' style='display:none'>
    
    
          <div style="margin:20px;">
            <div navbar style="display:flex;">
              <a href="javascript:void(0)" onclick="createContest()" style="text-decoration: none;">[Create]</a>
            </div>
            <h3>Create a contest</h3>
            <form>
              <label>Name: </label><br/>
              <input type="text"/><br/> 
              <label>Description: </label><br/>
              <textarea></textarea><br/>
              <input type="checkbox" id="publish"/>
              <label for="publish">Publish now</label><br/> 
              <label for="opening-time">Opening time:</label><br/>
              <input
                type="datetime-local"
                id="opening-time"
                name="opening-time"
                />
                <br/>
              <label for="closing-time">Closing time:</label><br/>
              <input
                type="datetime-local"
                id="closing-time"
                name="closing-time"
                />
            </form>
          </div>
    
    </div>`;
    // let btn = `<button id='${id}btn'>close ${id}</button>`;
    let root = $('#root2');
    root.append(element);
    // root.append(btn);
    let win = $(`#${id}`);

    // win.attr('data-options','onClose:closeHandler')

    win.window({
        width:600,
        height:400,
        modal:true
    });
    // $(`#${id}btn`).on('click',()=>win.window('destroy'));
    // $.fn.panel.defaults.onClose = ()=>console.log('closed');
    left = left + 10;
    top = top -10;
    win.window('move',{
        left,
        top
    })
    win.show();

    

}


// if(window.location == 'http://localhost:3000/admin'){
//     $('#root2').show();
//     $('#contestBtn').on('click',createContestWindow);
//     $('#groupBtn').on('click',createGroupWindow);
// }

});
