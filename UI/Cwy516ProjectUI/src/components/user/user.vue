<template>
    <div>
        <el-select placeholder="请选择" multiple collapse-tags v-model="checkedItemList" @change="changeSelect">
            <el-checkbox v-model="allChecked" @change="selectAll">全选</el-checkbox>
            <el-option v-for="(item,index) in checkDatas" :label="item.name" :value="item.value" :key="index" :disabled="true">
            </el-option>
        </el-select>
        <el-dialog title="用户" :visible.sync="formVisible" >
            <el-form ref="form" :model="form">
                <el-form-item label="用户名" prop="userName">
                    <el-input v-model="form.userName" placeholder="用户名"></el-input>
                </el-form-item>
                <el-form-item label="用户编号" prop="userCode">
                    <el-input v-model="form.userCode" placeholder="用户编号"></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="password">
                    <el-input v-model="form.password" placeholder="密码"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="Save">保存</el-button>
                    <el-button type="primary" @click="Cancel">取消</el-button>
                </el-form-item>
            </el-form>
        </el-dialog>
    </div>
</template>

<script>

export default{
    name:"user",
    data(){
        return {
            form:{
                userName:"",
                userCode:"",
                password:""
            },
            formVisible:false,
            checkDatas:[
                {name:"A",value:1},
                {name:"B",value:2},
                {name:"C",value:3}
            ],
            checkedItemList: [],
            allChecked: false
        }
    },
    methods:{
        changeSelect(){
            if(this.checkedItemList.length == this.checkDatas.length){
                this.allChecked = true;
            }
            else{
                this.allChecked = false;
            }
        },
        selectAll(val){
            if(val){
                this.checkedItemList = [];
                this.checkDatas.map(item=>this.checkedItemList.push(item.value));
            }
            else{
                this.checkedItemList = [];
            }
            //this.allChecked = val;//必须赋值，否则会出现第一次取消allChecked不生效
        },
        Save(){
            this.$request.post('home/AddUser',this.form).then(res=>{
                if(res.code==0){
                    this.formVisible=false;
                    this.$emit('ShowAllUser');
                }
                else{
                    Message({
                        type:'success',
                        message:JSON.stringify(res.messages)
                    });
                }
            })
        },
        Cancel(){
            this.formVisible = false;
        },
        Show(){
            this.formVisible = true;
        }
    }
}
</script>
